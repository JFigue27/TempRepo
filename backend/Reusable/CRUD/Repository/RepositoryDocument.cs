using LinqKit;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace Reusable
{
    public interface IDocumentRepository<T> : IRepository<T> where T : BaseDocument
    {
        void Activate(int id);
        void Deactivate(int id);
        void Deactivate(T entity);
        void Lock(int id);
        void Lock(T entity);
        void Unlock(int id);
        void Unlock(T entity);

        void Finalize(T entity);
        void Unfinalize(T entity);
    }

    public class DocumentRepository<T> : Repository<T>, IDocumentRepository<T> where T : BaseDocument
    {
        public DocumentRepository(DbContext context, LoggedUser loggedUser) : base(context, loggedUser)
        {
        }

        public override IEnumerable<T> GetAll(Expression<Func<T, object>> orderBy = null, params Expression<Func<T, object>>[] navigationProperties)
        {
            return GetList(orderBy, navigationProperties);
        }

        public override IEnumerable<T> GetList(Expression<Func<T, object>> orderBy, Expression<Func<T, object>>[] navigationProperties, params Expression<Func<T, bool>>[] wheres)
        {
            IQueryable<T> dbQuery = context.Set<T>();

            //Eager Loading:
            if (navigationProperties != null)
            {
                foreach (var navigationProperty in navigationProperties)
                {
                    dbQuery.Include(navigationProperty);
                }
            }

            //Wheres:
            var predicate = PredicateBuilder.New<T>(true);
            predicate = predicate.And(d => d.sys_active == true);

            foreach (var where in wheres)
            {
                predicate = predicate.And(where);
            }

            //Order By:
            if (orderBy != null)
            {
                dbQuery.AsExpandable().OrderBy(orderBy);
            }

            return dbQuery.Where(predicate);
        }

        public override T GetSingle(Expression<Func<T, object>>[] navigationProperties, params Expression<Func<T, bool>>[] wheres)
        {
            IQueryable<T> dbQuery = context.Set<T>();

            if (navigationProperties != null)
            {
                //Eager Loading:
                foreach (var navigationProperty in navigationProperties)
                {
                    dbQuery.Include(navigationProperty);
                }
            }

            //Wheres:
            var predicate = PredicateBuilder.New<T>(true);
            predicate = predicate.And(d => d.sys_active == true);

            foreach (var where in wheres)
            {
                predicate = predicate.And(where);
            }


            return dbQuery.Where(predicate).FirstOrDefault();
        }

        public override T GetByID(int id, params Expression<Func<T, object>>[] navigationProperties)
        {
            DbSet<T> dbQuery = context.Set<T>();

            if (navigationProperties != null)
            {
                //Eager Loading:
                foreach (var navigationProperty in navigationProperties)
                {
                    dbQuery.Include(navigationProperty);
                }
            }

            dbQuery.Where(d => d.sys_active == true);

            return dbQuery.Find(id);
        }

        public override IEnumerable<T> GetListByParent<P>(int parentID, params Expression<Func<T, object>>[] navigationProperties)
        {
            List<T> list = new List<T>();

            DbSet<P> setParent = context.Set<P>();

            P parent = setParent.Find(parentID);

            if (parent == null)
            {
                throw new Exception("Parent non-existent.");
            }

            if (parent is BaseDocument)
            {
                if ((parent as BaseDocument).sys_active == false)
                {
                    throw new Exception("Parent non-existent.");
                }
            }

            string tName = typeof(T).Name + "s";
            IQueryable<T> dbQuery = context.Entry(parent).Collection<T>(tName)
                .Query();

            if (navigationProperties != null)
            {
                //Eager Loading:
                foreach (var navigationProperty in navigationProperties)
                {
                    dbQuery.Include(navigationProperty);
                }
            }

            dbQuery.Where(d => d.sys_active == true);

            list = dbQuery.ToList();

            //Removing Recursivity
            string navigationPropertyName = typeof(P).Name;
            foreach (T item in list)
            {
                try
                {
                    //Trying for collection
                    context.Entry(item).Collection<P>(navigationPropertyName + "s").CurrentValue.Clear();
                }
                catch (Exception)
                {
                    //Trying for reference
                    //context.Entry(item).Reference<P>(navigationPropertyName).CurrentValue.
                }
            }

            return list;
        }

        public override T GetSingleByParent<P>(int parentID, params Expression<Func<T, object>>[] navigationProperties)
        {
            DbSet<P> setParent = context.Set<P>();

            P parent = setParent.Find(parentID);

            if (parent == null)
            {
                throw new Exception("Parent non-existent.");
            }

            if (parent is BaseDocument)
            {
                if ((parent as BaseDocument).sys_active == false)
                {
                    throw new Exception("Parent non-existent.");
                }
            }

            string tName = typeof(T).Name;
            IQueryable<T> dbQuery = context.Entry(parent).Reference<T>(tName)
                    .Query()
                    .AsNoTracking();

            if (navigationProperties != null)
            {
                //Eager Loading:
                foreach (var navigationProperty in navigationProperties)
                {
                    dbQuery.Include(navigationProperty);
                }
            }

            return dbQuery.FirstOrDefault();
        }



        public override void Add(params T[] items)
        {
            DbSet<T> dbSet = context.Set<T>();
            foreach (T item in items)
            {
                dbSet.Attach(item);
            }

            foreach (DbEntityEntry<BaseDocument> entry in context.ChangeTracker.Entries<BaseDocument>())
            {
                context.Entry(entry.Entity).State = EntityState.Unchanged;
            }

            foreach (T item in items)
            {
                context.Entry(item).State = EntityState.Added;
            }

            context.SaveChanges();

            foreach (T document in items)
            {
                document.InfoTrack = new Track()
                {
                    Date_CreatedOn = DateTimeOffset.Now,
                    Entity_ID = document.id,
                    Entity_Kind = document.AAA_EntityName,
                    User_CreatedByKey = ByUserId,
                    User_AssignedToKey = ByUserId
                };
                context.Entry(document.InfoTrack).State = EntityState.Added;
            }
            context.SaveChanges();
        }

        public override void Delete(int id)
        {
            Deactivate(id);
        }

        public override void Delete(T document)
        {
            Deactivate(document);
        }

        public override void Update(T document) //Update only parent Entity, not children
        {
            DbSet<T> dbSet = context.Set<T>();

            T originalDocument = dbSet.Find(document.id);
            if (originalDocument == null)
            {
                throw new Exception("Entity of type: [" + document.AAA_EntityName + "] with ID: [" + document.id + "] was not found.");
            }

            context.Entry(originalDocument).CurrentValues.SetValues(document);
            context.Entry(originalDocument).State = EntityState.Modified;

            /*DOCUMENT*/
            originalDocument.InfoTrack = context.Set<Track>()
                                .AsNoTracking()
                                .FirstOrDefault(t => t.Entity_ID == originalDocument.id && t.Entity_Kind == originalDocument.AAA_EntityName);

            if (originalDocument.InfoTrack != null)
            {
                originalDocument.InfoTrack.User_LastEditedByKey = ByUserId;
                originalDocument.InfoTrack.Date_EditedOn = DateTimeOffset.Now;

                context.Entry(originalDocument.InfoTrack).State = EntityState.Modified;
            }

            context.SaveChanges();
        }

        public override P AddToParent<P>(int parentId, T document)
        {
            //string sParentPropID = typeof(P).Name + "Key";
            //P parent = context.Database.SqlQuery<P>("select * from " + typeof(P).Name + " where " + sParentPropID + " = @p0", parentId).FirstOrDefault();

            DbSet<P> parentSet = context.Set<P>();
            P parent = parentSet.Find(parentId);
            if (parent == null)
            {
                throw new Exception("Non-existent Parent Entity.");
            }
            if (parent is BaseDocument)
            {
                if ((parent as BaseDocument).sys_active == false)
                {
                    throw new Exception("Non-existent Parent Entity.");
                }
            }

            //parentSet.Attach(parent);
            //context.Entry(parent).State = EntityState.Unchanged;


            string navigationPropertyName = typeof(T).Name;
            DbMemberEntry memberEntry;
            try
            {
                memberEntry = context.Entry(parent).Member(navigationPropertyName);
            }
            catch (Exception)
            {
                try
                {
                    navigationPropertyName += "s";
                    memberEntry = context.Entry(parent).Member(navigationPropertyName);
                }
                catch (Exception)
                {
                    throw new KnownError("Property not found: [" + navigationPropertyName + "]");
                }
            }

            if (memberEntry is DbCollectionEntry)
            {
                DbCollectionEntry<P, T> childrenCollection = context.Entry(parent).Collection<T>(navigationPropertyName);
                childrenCollection.Load();

                if (!childrenCollection.CurrentValue.Contains(document))
                {
                    childrenCollection.CurrentValue.Add(document);
                    if (document.id > 0)
                    {
                        context.Entry(document).State = EntityState.Unchanged;

                        document.InfoTrack = context.Set<Track>()
                                            .AsNoTracking()
                                            .FirstOrDefault(t => t.Entity_ID == document.id && t.Entity_Kind == document.AAA_EntityName);

                        if (document.InfoTrack != null)
                        {
                            document.InfoTrack.User_LastEditedByKey = ByUserId;
                            document.InfoTrack.Date_EditedOn = DateTimeOffset.Now;

                            context.Entry(document.InfoTrack).State = EntityState.Modified;
                        }

                    }
                    else
                    {
                        context.Entry(document).State = EntityState.Added;

                        document.InfoTrack = new Track()
                        {
                            Date_CreatedOn = DateTimeOffset.Now,
                            Entity_ID = document.id,
                            Entity_Kind = document.AAA_EntityName,
                            User_CreatedByKey = ByUserId,
                            User_AssignedToKey = ByUserId
                        };
                        context.Entry(document.InfoTrack).State = EntityState.Added;
                    }

                    context.SaveChanges();
                }
            }
            else if (memberEntry is DbReferenceEntry)
            {
                DbReferenceEntry<P, T> childrenReference = context.Entry(parent).Reference<T>(navigationPropertyName);
                childrenReference.Load();

                if (childrenReference.CurrentValue == null || !childrenReference.CurrentValue.Equals(document))
                {
                    childrenReference.CurrentValue = document;
                    if (document.id > 0)
                    {
                        context.Entry(document).State = EntityState.Unchanged;

                        document.InfoTrack = context.Set<Track>()
                                            .AsNoTracking()
                                            .FirstOrDefault(t => t.Entity_ID == document.id && t.Entity_Kind == document.AAA_EntityName);

                        if (document.InfoTrack != null)
                        {
                            document.InfoTrack.User_LastEditedByKey = ByUserId;
                            document.InfoTrack.Date_EditedOn = DateTimeOffset.Now;

                            context.Entry(document.InfoTrack).State = EntityState.Modified;
                        }
                    }
                    else
                    {
                        context.Entry(document).State = EntityState.Added;

                        document.InfoTrack = new Track()
                        {
                            Date_CreatedOn = DateTimeOffset.Now,
                            Entity_ID = document.id,
                            Entity_Kind = document.AAA_EntityName,
                            User_CreatedByKey = ByUserId,
                            User_AssignedToKey = ByUserId
                        };
                        context.Entry(document.InfoTrack).State = EntityState.Added;
                    }

                    context.SaveChanges();
                }
            }

            return parent;
        }

        public override T SetPropertyValue(int documentId, string sProperty, string newValue)
        {
            DbSet<T> documentSet = context.Set<T>();
            T document = documentSet.Find(documentId);

            if (document == null)
            {
                throw new KnownError("Non-existent Document.");
            }
            if (document.sys_active == false)
            {
                throw new KnownError("Non-existent Document.");
            }


            SetValueFromString(document, sProperty, newValue);

            context.Entry(document).State = EntityState.Modified;

            document.InfoTrack = context.Set<Track>()
                                .AsNoTracking()
                                .FirstOrDefault(t => t.Entity_ID == document.id && t.Entity_Kind == document.AAA_EntityName);

            if (document.InfoTrack != null)
            {
                document.InfoTrack.User_LastEditedByKey = ByUserId;
                document.InfoTrack.Date_EditedOn = DateTimeOffset.Now;

                context.Entry(document.InfoTrack).State = EntityState.Modified;
            }

            context.SaveChanges();

            return document;
        }

        public override void RemoveFromParent<P>(int parentId, T document)
        {
            DbSet<P> parentSet = context.Set<P>();
            P parent = parentSet.Find(parentId);
            if (parent == null)
            {
                throw new Exception("Non-existent Parent Entity.");
            }
            if (parent is BaseDocument)
            {
                if ((parent as BaseDocument).sys_active == false)
                {
                    throw new Exception("Non-existent Parent Entity.");
                }
            }

            string navigationPropertyName = typeof(T).Name + "s";

            DbCollectionEntry<P, T> childrenCollection = context.Entry(parent).Collection<T>(navigationPropertyName);
            childrenCollection.Load();

            if (childrenCollection.CurrentValue.Contains(document))
            {
                childrenCollection.CurrentValue.Remove(document);
                context.SaveChanges();

                document.InfoTrack = context.Set<Track>()
                                    .AsNoTracking()
                                    .FirstOrDefault(t => t.Entity_ID == document.id && t.Entity_Kind == document.AAA_EntityName);

                if (document.InfoTrack != null)
                {
                    document.InfoTrack.User_LastEditedByKey = ByUserId;
                    document.InfoTrack.Date_EditedOn = DateTimeOffset.Now;

                    context.Entry(document.InfoTrack).State = EntityState.Modified;
                }

                context.SaveChanges();
            }
        }

        public void Activate(int id)
        {
            DbSet<T> tSet = context.Set<T>();

            T document = tSet.Find(id);

            if (document != null)
            {
                document.sys_active = true;
                context.Entry(document).State = EntityState.Modified;

                document.InfoTrack = context.Set<Track>()
                                    .AsNoTracking()
                                    .FirstOrDefault(t => t.Entity_ID == document.id && t.Entity_Kind == document.AAA_EntityName);

                if (document.InfoTrack != null)
                {
                    document.InfoTrack.Date_EditedOn = DateTimeOffset.Now;
                    document.InfoTrack.User_LastEditedByKey = ByUserId;

                    document.InfoTrack.Date_RemovedOn = null;
                    document.InfoTrack.User_RemovedByKey = null;

                    context.Entry(document.InfoTrack).State = EntityState.Modified;
                }

                context.SaveChanges();
            }
            else
            {
                throw new Exception("Document not found.");
            }
        }

        public void Deactivate(int id)
        {
            DbSet<T> tSet = context.Set<T>();

            T document = tSet.Find(id);

            if (document != null)
            {
                document.sys_active = false;

                context.Entry(document).State = EntityState.Modified;

                document.InfoTrack = context.Set<Track>()
                                    .AsNoTracking()
                                    .FirstOrDefault(t => t.Entity_ID == document.id && t.Entity_Kind == document.AAA_EntityName);

                if (document.InfoTrack != null)
                {
                    document.InfoTrack.Date_RemovedOn = DateTimeOffset.Now;
                    document.InfoTrack.User_RemovedByKey = ByUserId;

                    context.Entry(document.InfoTrack).State = EntityState.Modified;
                }

                context.SaveChanges();
            }
            else
            {
                throw new Exception("Document not found.");
            }
        }

        public void Deactivate(T document)
        {
            if (document != null)
            {
                if (context.Entry(document).State == EntityState.Detached)
                {
                    document = context.Set<T>().Find(document.id);
                }

                document.sys_active = false;

                context.Entry(document).State = EntityState.Modified;

                document.InfoTrack = context.Set<Track>()
                                    .AsNoTracking()
                                    .FirstOrDefault(t => t.Entity_ID == document.id && t.Entity_Kind == document.AAA_EntityName);

                if (document.InfoTrack != null)
                {
                    document.InfoTrack.Date_RemovedOn = DateTimeOffset.Now;
                    document.InfoTrack.User_RemovedByKey = ByUserId;

                    context.Entry(document.InfoTrack).State = EntityState.Modified;
                }

                context.SaveChanges();
            }
            else
            {
                throw new Exception("Document not found.");
            }
        }

        public override void RemoveAllWhere(params Expression<Func<T, bool>>[] wheres)
        {
            var predicate = PredicateBuilder.New<T>(true);

            foreach (var where in wheres)
            {
                predicate = predicate.And(where);
            }

            IEnumerable<T> list;
            IQueryable<T> dbQuery = context.Set<T>();

            list = dbQuery.AsExpandable()
            .AsNoTracking()
            .Where(predicate);

            list = list.ToList();

            foreach (T item in list)
            {
                Delete(item.id);
            }
        }

        public void Lock(int id)
        {
            DbSet<T> tSet = context.Set<T>();

            T document = tSet.Find(id);

            if (document != null)
            {
                document.is_locked = true;

                context.Entry(document).State = EntityState.Modified;

                document.InfoTrack = context.Set<Track>()
                                    .AsNoTracking()
                                    .FirstOrDefault(t => t.Entity_ID == document.id && t.Entity_Kind == document.AAA_EntityName);

                if (document.InfoTrack != null)
                {
                    document.InfoTrack.Date_EditedOn = DateTimeOffset.Now;
                    document.InfoTrack.User_LastEditedByKey = ByUserId;

                    context.Entry(document.InfoTrack).State = EntityState.Modified;
                }

                context.SaveChanges();
            }
            else
            {
                throw new KnownError("Document not found.");
            }
        }

        public void Lock(T document)
        {
            if (document != null)
            {
                document.is_locked = true;

                context.Entry(document).State = EntityState.Modified;

                document.InfoTrack = context.Set<Track>()
                                    .AsNoTracking()
                                    .FirstOrDefault(t => t.Entity_ID == document.id && t.Entity_Kind == document.AAA_EntityName);

                if (document.InfoTrack != null)
                {
                    document.InfoTrack.Date_EditedOn = DateTimeOffset.Now;
                    document.InfoTrack.User_LastEditedByKey = ByUserId;

                    context.Entry(document.InfoTrack).State = EntityState.Modified;
                }

                context.SaveChanges();
            }
            else
            {
                throw new KnownError("Document not found.");
            }
        }

        public void Unlock(int id)
        {
            DbSet<T> tSet = context.Set<T>();

            T document = tSet.Find(id);

            if (document != null)
            {
                document.is_locked = false;

                context.Entry(document).State = EntityState.Modified;

                document.InfoTrack = context.Set<Track>()
                                    .AsNoTracking()
                                    .FirstOrDefault(t => t.Entity_ID == document.id && t.Entity_Kind == document.AAA_EntityName);

                if (document.InfoTrack != null)
                {
                    document.InfoTrack.Date_EditedOn = DateTimeOffset.Now;
                    document.InfoTrack.User_LastEditedByKey = ByUserId;

                    context.Entry(document.InfoTrack).State = EntityState.Modified;
                }

                context.SaveChanges();
            }
            else
            {
                throw new KnownError("Document not found.");
            }
        }

        public void Unlock(T document)
        {
            if (document != null)
            {
                document.is_locked = false;

                context.Entry(document).State = EntityState.Modified;

                document.InfoTrack = context.Set<Track>()
                                    .AsNoTracking()
                                    .FirstOrDefault(t => t.Entity_ID == document.id && t.Entity_Kind == document.AAA_EntityName);

                if (document.InfoTrack != null)
                {
                    document.InfoTrack.Date_EditedOn = DateTimeOffset.Now;
                    document.InfoTrack.User_LastEditedByKey = ByUserId;

                    context.Entry(document.InfoTrack).State = EntityState.Modified;
                }

                context.SaveChanges();
            }
            else
            {
                throw new KnownError("Document not found.");
            }
        }

        public void Finalize(T document)
        {
            if (document != null)
            {
                T originalFromDB = context.Set<T>().Find(document.id);
                if (originalFromDB == null || (originalFromDB as BaseDocument).sys_active == false)
                {
                    throw new KnownError("Non-Existent Document");
                }

                if ((originalFromDB as BaseDocument).document_status == "FINALIZED")
                {
                    throw new KnownError("Record was already Finalized.");
                }

                //foreach (DbEntityEntry<BaseEntity> entry in context.ChangeTracker.Entries<BaseEntity>())
                //{
                //    context.Entry(entry.Entity).State = EntityState.Detached;
                //}

                document.document_status = "FINALIZED";
                document.is_locked = true;

                context.Entry(document).State = EntityState.Modified;

                document.InfoTrack = context.Set<Track>()
                                    .AsNoTracking()
                                    .FirstOrDefault(t => t.Entity_ID == document.id && t.Entity_Kind == document.AAA_EntityName);

                if (document.InfoTrack != null)
                {
                    document.InfoTrack.Date_EditedOn = DateTimeOffset.Now;
                    document.InfoTrack.User_LastEditedByKey = ByUserId;

                    context.Entry(document.InfoTrack).State = EntityState.Modified;
                }

                context.SaveChanges();
            }
            else
            {
                throw new KnownError("Document not found.");
            }
        }

        public void Unfinalize(T document)
        {
            if (document != null)
            {
                document.document_status = "IN PROGRESS";
                document.is_locked = false;

                context.Entry(document).State = EntityState.Modified;

                document.InfoTrack = context.Set<Track>()
                                    .AsNoTracking()
                                    .FirstOrDefault(t => t.Entity_ID == document.id && t.Entity_Kind == document.AAA_EntityName);

                if (document.InfoTrack != null)
                {
                    document.InfoTrack.Date_EditedOn = DateTimeOffset.Now;
                    document.InfoTrack.User_LastEditedByKey = ByUserId;

                    context.Entry(document.InfoTrack).State = EntityState.Modified;
                }

                context.SaveChanges();
            }
            else
            {
                throw new KnownError("Document not found.");
            }
        }

    }
}