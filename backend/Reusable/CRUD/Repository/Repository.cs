using LinqKit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Reusable
{
    public interface IRepository<T> : IReadOnlyRepository<T> where T : BaseEntity
    {
        void Add(params T[] items);
        void Update(T item);
        void Delete(int id);
        void Delete(T entity);

        P AddToParent<P>(int parentId, T entity) where P : class;
        void RemoveFromParent<P>(int parentId, T entity) where P : class;
        void RemoveAllWhere(params Expression<Func<T, bool>>[] wheres);
        T SetPropertyValue(int entityId, string sProperty, string newValue);
    }

    public class Repository<T> : ReadOnlyRepository<T>, IRepository<T> where T : BaseEntity
    {
        public Repository(DbContext context, LoggedUser loggedUser) : base(context, loggedUser)
        {
        }

        public virtual void Add(params T[] items)
        {
            DbSet<T> dbSet = context.Set<T>();
            foreach (T item in items)
            {
                dbSet.Attach(item);
            }

            foreach (DbEntityEntry<BaseEntity> entry in context.ChangeTracker.Entries<BaseEntity>())
            {
                context.Entry(entry.Entity).State = EntityState.Unchanged;
            }

            foreach (T item in items)
            {
                context.Entry(item).State = EntityState.Added;
            }

            context.SaveChanges();
        }

        public virtual void Delete(int id)
        {
            DbSet<T> tSet = context.Set<T>();
            T entity = tSet.Find(id);

            if (entity != null)
            {
                context.Entry(entity).State = EntityState.Deleted;
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Entity not found.");
            }
        }

        public virtual void Delete(T entity)
        {
            if (entity != null)
            {
                //foreach (var item in context.ChangeTracker.Entries())
                //{
                //    context.Entry(item.Entity).State = EntityState.Detached;
                //}

                context.Entry(entity).State = EntityState.Deleted;
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Entity not found.");
            }
        }

        public virtual void Update(T item) //Update only parent Entity, not children
        {
            DbSet<T> dbSet = context.Set<T>();

            T originalEntity = dbSet.Find(item.id);
            if (originalEntity == null)
            {
                throw new Exception("Entity of type: [" + item.AAA_EntityName + "] with ID: [" + item.id + "] was not found.");
            }

            context.Entry(originalEntity).CurrentValues.SetValues(item);
            context.Entry(originalEntity).State = EntityState.Modified;

            context.SaveChanges();
        }

        public virtual P AddToParent<P>(int parentId, T entity) where P : class
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

                if (!childrenCollection.CurrentValue.Contains(entity))
                {
                    childrenCollection.CurrentValue.Add(entity);
                    if (entity.id > 0)
                    {
                        context.Entry(entity).State = EntityState.Unchanged;
                    }
                    else
                    {
                        context.Entry(entity).State = EntityState.Added;
                    }

                    context.SaveChanges();
                }
            }
            else if (memberEntry is DbReferenceEntry)
            {
                DbReferenceEntry<P, T> childrenReference = context.Entry(parent).Reference<T>(navigationPropertyName);
                childrenReference.Load();

                if (childrenReference.CurrentValue == null || !childrenReference.CurrentValue.Equals(entity))
                {
                    childrenReference.CurrentValue = entity;
                    if (entity.id > 0)
                    {
                        context.Entry(entity).State = EntityState.Unchanged;
                    }
                    else
                    {
                        context.Entry(entity).State = EntityState.Added;
                    }

                    context.SaveChanges();
                }
            }

            //DbSet<T> entitySet = context.Set<T>();
            //entitySet.Attach(entity);

            //PropertyInfo navigationProperty = parent.GetType().GetProperty(navigationPropertyName, BindingFlags.Public | BindingFlags.Instance);
            //ICollection<T> childrenList = (ICollection<T>)navigationProperty.GetValue(entity);

            //childrenList.Add(entity);

            return parent;
        }

        public virtual T SetPropertyValue(int entityId, string sProperty, string newValue)
        {
            DbSet<T> entitySet = context.Set<T>();
            T entity = entitySet.Find(entityId);

            if (entity == null)
            {
                throw new KnownError("Non-existent Parent Entity.");
            }
            if (entity is BaseDocument)
            {
                if ((entity as BaseDocument).sys_active == false)
                {
                    throw new KnownError("Non-existent Parent Entity.");
                }
            }

            SetValueFromString(entity, sProperty, newValue);

            context.Entry(entity).State = EntityState.Modified;

            context.SaveChanges();

            return entity;
        }

        /// <summary>
        /// Sets a value in an object, used to hide all the logic that goes into
        ///     handling this sort of thing, so that is works elegantly in a single line.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        protected void SetValueFromString(object target, string propertyName, string propertyValue)
        {
            PropertyInfo oProp = target.GetType().GetProperty(propertyName);
            Type tProp = oProp.PropertyType;

            //Nullable properties have to be treated differently, since we 
            //  use their underlying property to set the value in the object
            if (tProp.IsGenericType
                && tProp.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                //if it's null, just set the value from the reserved word null, and return
                if (propertyValue == null)
                {
                    oProp.SetValue(target, null, null);
                    return;
                }

                //Get the underlying type property instead of the nullable generic
                tProp = new NullableConverter(oProp.PropertyType).UnderlyingType;
            }

            //use the converter to get the correct value
            oProp.SetValue(target, Convert.ChangeType(propertyValue, tProp), null);
        }

        public virtual void RemoveFromParent<P>(int parentId, T entity) where P : class
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

            if (childrenCollection.CurrentValue.Contains(entity))
            {
                childrenCollection.CurrentValue.Remove(entity);
                context.SaveChanges();
            }
        }

        public virtual void RemoveAllWhere(params Expression<Func<T, bool>>[] wheres)
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
    }
}
