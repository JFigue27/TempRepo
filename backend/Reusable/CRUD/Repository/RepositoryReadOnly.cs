using LinqKit;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Reusable
{
    public interface IReadOnlyRepository<T> where T : class
    {
        int? ByUserId { get; set; }
        string EntityName { get; set; }

        IEnumerable<T> GetAll(Expression<Func<T, object>> orderBy = null, params Expression<Func<T, object>>[] navigationProperties);
        IEnumerable<T> GetList(Expression<Func<T, object>> orderBy, Expression<Func<T, object>>[] navigationProperties, params Expression<Func<T, bool>>[] wheres);
        T GetByID(int id, params Expression<Func<T, object>>[] navigationProperties);
        T GetSingle(Expression<Func<T, object>>[] navigationProperties, params Expression<Func<T, bool>>[] wheres);

        IEnumerable<T> GetListByParent<P>(int parentID, params Expression<Func<T, object>>[] navigationProperties) where P : class;
        IList<T> GetListByParent<P>(int parentID, string customProperty, params Expression<Func<T, object>>[] navigationProperties) where P : class;
        T GetSingleByParent<P>(int parentID, params Expression<Func<T, object>>[] navigationProperties) where P : class;
    }

    public class ReadOnlyRepository<T> : IReadOnlyRepository<T> where T : class
    {
        protected readonly DbContext context;
        public int? ByUserId { get; set; }
        protected LoggedUser loggedUser { get; set; }

        public string EntityName { get; set; }

        public ReadOnlyRepository(DbContext context, LoggedUser loggedUser)
        {
            this.context = context;
            EntityName = typeof(T).Name;
            this.loggedUser = loggedUser;
            ByUserId = this.loggedUser?.UserID;
        }

        public virtual IEnumerable<T> GetAll(Expression<Func<T, object>> orderBy = null, params Expression<Func<T, object>>[] navigationProperties)
        {
            return GetList(orderBy, navigationProperties);
        }

        public virtual IEnumerable<T> GetList(Expression<Func<T, object>> orderBy, Expression<Func<T, object>>[] navigationProperties, params Expression<Func<T, bool>>[] wheres)
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

            if (wheres != null)
            {
                foreach (var where in wheres)
                {
                    predicate = predicate.And(where);
                }
            }

            //Order By:
            if (orderBy != null)
            {
                dbQuery.AsExpandable().OrderBy(orderBy);
            }

            return dbQuery.Where(predicate);
        }

        public virtual T GetSingle(Expression<Func<T, object>>[] navigationProperties, params Expression<Func<T, bool>>[] wheres)
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
            foreach (var where in wheres)
            {
                predicate = predicate.And(where);
            }

            return dbQuery.Where(predicate).FirstOrDefault();
        }

        public virtual T GetByID(int id, params Expression<Func<T, object>>[] navigationProperties)
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

            return dbQuery.Find(id);
        }

        public virtual IEnumerable<T> GetListByParent<P>(int parentID, params Expression<Func<T, object>>[] navigationProperties) where P : class
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

            list = dbQuery.AsNoTracking().ToList();

            //Removing Recurivity
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

        public virtual T GetSingleByParent<P>(int parentID, params Expression<Func<T, object>>[] navigationProperties) where P : class
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

        public IList<T> GetListByParent<P>(int parentID, string customProperty, params Expression<Func<T, object>>[] navigationProperties) where P : class
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

            IQueryable<T> dbQuery = context.Entry(parent).Collection<T>(customProperty)
                .Query().AsExpandable();

            if (navigationProperties != null)
            {
                //Eager Loading:
                foreach (var navigationProperty in navigationProperties)
                {
                    dbQuery = dbQuery.Include(navigationProperty);
                }
            }

            list = dbQuery.AsNoTracking().ToList();

            /*DOCUMENT*/
            if (typeof(T).IsSubclassOf(typeof(BaseDocument)))
            {
                list = list.Where(d => (d as BaseDocument).sys_active == true).ToList();

                foreach (T item in list)
                {
                    var document = item as BaseDocument;

                    document.InfoTrack = context.Set<Track>()
                                        .AsNoTracking()
                                        .FirstOrDefault(t => t.Entity_ID == document.id && t.Entity_Kind == document.AAA_EntityName);

                }
            }

            //Removing Recurivity
            /*string navigationPropertyName = typeof(P).Name;
            foreach (T item in list)
            {
                //PropertyInfo prop = item.GetType().GetProperty(navigationPropertyName, BindingFlags.Public | BindingFlags.Instance);
                //if (null != prop && prop.CanWrite)
                //{
                //    prop.SetValue(item, new List<P>());
                //}

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

            }*/

            return list;
        }
    }
}
