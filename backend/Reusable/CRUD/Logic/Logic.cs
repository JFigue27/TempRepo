using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

namespace Reusable
{
    public interface ILogic<Entity> : IReadOnlyLogic<Entity> where Entity : BaseEntity
    {
        CommonResponse AddTransaction(Entity entity);
        void Add(Entity entity);

        CommonResponse Remove(Entity id);
        CommonResponse Remove(int id);
        CommonResponse UpdateTransaction(Entity entity);
        CommonResponse AddToParent<ParentType>(int parentID, Entity entity) where ParentType : BaseEntity;
        CommonResponse RemoveFromParent<Parent>(int parentID, Entity entity) where Parent : BaseEntity;
        CommonResponse SetPropertyValue(Entity entity, string sProperty, string value);
        CommonResponse GetAvailableFor<ForEntity>(int id) where ForEntity : BaseEntity;
    }

    public abstract class Logic<Entity> : ReadOnlyLogic<Entity>, ILogic<Entity> where Entity : BaseEntity, new()
    {
        protected new IRepository<Entity> repository;

        public Logic(DbContext context, IRepository<Entity> repository, LoggedUser LoggedUser) : base(context, repository, LoggedUser)
        {
            this.repository = repository;
        }

        protected enum OPERATION_MODE
        {
            NONE,
            ADD,
            UPDATE
        };

        protected virtual void onAfterSaving(DbContext context, Entity entity, BaseEntity parent = null, OPERATION_MODE mode = OPERATION_MODE.NONE) { }
        protected virtual void onBeforeSaving(Entity entity, BaseEntity parent = null, OPERATION_MODE mode = OPERATION_MODE.NONE) { }
        protected virtual void onBeforeRemoving(Entity entity, BaseEntity parent = null) { }
        protected virtual void onFinalize(Entity entity) { }
        protected virtual void onUnfinalize(Entity entity) { }

        public virtual CommonResponse AddTransaction(Entity entity)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Add(entity);

                        transaction.Commit();
                    }
                    catch (KnownError error)
                    {
                        transaction.Rollback();
                        return response.Error(error);
                    }
                    catch (CannotCreateBecauseAlreadyExists error)
                    {
                        transaction.Rollback();
                        return response.Success(error.ExistingEntity);
                    }
                    catch (DbEntityValidationException ex)
                    {
                        // Retrieve the error messages as a list of strings.
                        var errorMessages = ex.EntityValidationErrors
                                .SelectMany(x => x.ValidationErrors)
                                .Select(x => x.ErrorMessage);

                        // Join the list to a single string.
                        var fullErrorMessage = string.Join("; ", errorMessages);

                        transaction.Rollback();
                        return response.Error(fullErrorMessage);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return response.Error(ex.ToString());
                    }
                }
            }
            catch (KnownError error)
            {
                return response.Error(error);
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }

            return response.Success(entity);
        }

        public virtual void Add(Entity entity)
        {
            onBeforeSaving(entity, null, OPERATION_MODE.ADD);

            repository.Add(entity);

            onAfterSaving(context, entity, null, OPERATION_MODE.ADD);
        }

        public virtual CommonResponse Remove(Entity entity)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        //repository.ByUserId = LoggedUser.UserID;
                        onBeforeRemoving(entity);
                        repository.Delete(entity);

                        transaction.Commit();
                    }
                    catch (KnownError error)
                    {
                        transaction.Rollback();
                        return response.Error(error);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return response.Error("ERROR: " + e.ToString());
                    }
                }

            }
            catch (KnownError error)
            {
                return response.Error(error);
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }
            return response.Success(entity, repository.EntityName + " removed successfully.");
        }

        public virtual CommonResponse Remove(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        //var repository = RepositoryFactory.Create<Entity>(context, ByUserId);
                        //repository.ByUserId = LoggedUser.UserID;
                        repository.Delete(id);

                        transaction.Commit();
                    }
                    catch (KnownError error)
                    {
                        transaction.Rollback();
                        return response.Error(error);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return response.Error("ERROR: " + e.ToString());
                    }
                }

            }
            catch (KnownError error)
            {
                return response.Error(error);
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }
            return response.Success(id, repository.EntityName + " removed successfully.");
        }

        public virtual void Update(Entity entity)
        {
            onBeforeSaving(entity, null, OPERATION_MODE.UPDATE);

            repository.Update(entity);

            onAfterSaving(context, entity, null, OPERATION_MODE.UPDATE);
        }

        public virtual CommonResponse UpdateTransaction(Entity entity)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Update(entity);

                        transaction.Commit();

                        AdapterOut(entity);
                    }
                    catch (KnownError error)
                    {
                        transaction.Rollback();
                        return response.Error(error);
                    }
                    catch (DbEntityValidationException ex)
                    {
                        // Retrieve the error messages as a list of strings.
                        var errorMessages = ex.EntityValidationErrors
                                .SelectMany(x => x.ValidationErrors)
                                .Select(x => x.ErrorMessage);

                        // Join the list to a single string.
                        var fullErrorMessage = string.Join("; ", errorMessages);

                        transaction.Rollback();
                        return response.Error(fullErrorMessage);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return response.Error("ERROR: " + e.ToString());
                    }
                }

            }
            catch (KnownError error)
            {
                return response.Error(error);
            }
            catch (Exception e)
            {

                return response.Error("ERROR: " + e.ToString());
            }

            return response.Success(entity);
        }

        public virtual CommonResponse AddToParent<ParentType>(int parentID, Entity entity) where ParentType : BaseEntity
        {
            CommonResponse response = new CommonResponse();
            try
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        //var repository = RepositoryFactory.Create<Entity>(context, ByUserId);

                        //var parentRepoType = typeof(BaseEntityRepository<>);
                        //Type[] parentRepositoryArgs = { typeof(ParentType) };
                        //var makeme = parentRepoType.MakeGenericType(parentRepositoryArgs);
                        //object parentRepository = Activator.CreateInstance(makeme);

                        //PropertyInfo propContext = parentRepository.GetType().GetProperty("context", BindingFlags.Public | BindingFlags.Instance);
                        //propContext.SetValue(parentRepository, context);

                        //PropertyInfo propByUser = parentRepository.GetType().GetProperty("ByUserId", BindingFlags.Public | BindingFlags.Instance);
                        //propByUser.SetValue(parentRepository, ByUserId);

                        //MethodInfo method = parentRepository.GetType().GetMethod("GetByID");
                        //BaseEntity parent = (Entity)method.Invoke(parentRepository, new object[] { parentID });
                        //if (parent == null)
                        //{
                        //    return response.Error("Non-existent Parent Entity.");
                        //}

                        //repository.ByUserId = LoggedUser.UserID;
                        ParentType parent = repository.AddToParent<ParentType>(parentID, entity);
                        onAfterSaving(context, entity, parent, OPERATION_MODE.ADD);

                        transaction.Commit();
                    }
                    catch (KnownError error)
                    {
                        transaction.Rollback();
                        return response.Error(error);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return response.Error("ERROR: " + e.ToString());
                    }
                }

            }
            catch (KnownError error)
            {
                return response.Error(error);
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }

            return response.Success(entity);
        }

        public virtual CommonResponse SetPropertyValue(Entity entity, string sProperty, string value)
        {
            CommonResponse response = new CommonResponse();
            try
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        //repository.ByUserId = LoggedUser.UserID;

                        Entity result = repository.SetPropertyValue(entity.id, sProperty, value);

                        onAfterSaving(context, entity, null, OPERATION_MODE.UPDATE);

                        transaction.Commit();
                    }
                    catch (KnownError error)
                    {
                        transaction.Rollback();
                        return response.Error(error);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return response.Error("ERROR: " + e.ToString());
                    }
                }

            }
            catch (KnownError error)
            {
                return response.Error(error);
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }

            return response.Success(entity);
        }

        public virtual CommonResponse GetAvailableFor<ForEntity>(int id) where ForEntity : BaseEntity
        {
            CommonResponse response = new CommonResponse();
            IEnumerable<Entity> availableEntities;
            try
            {
                //repository.ByUserId = LoggedUser.UserID;

                IRepository<ForEntity> oRepository = new Repository<ForEntity>(context, LoggedUser);
                //oRepository.ByUserId = LoggedUser.UserID;


                ForEntity forEntity = oRepository.GetByID(id);
                if (forEntity == null)
                {
                    throw new Exception("Entity " + forEntity.AAA_EntityName + " not found.");
                }

                IEnumerable<Entity> childrenInForEntity = repository.GetListByParent<ForEntity>(id);

                IEnumerable<Entity> allEntities = repository.GetAll();

                availableEntities = allEntities.Where(e => !childrenInForEntity.Any(o => o.id == e.id));

                AdapterOut(availableEntities.ToArray());
                return response.Success(availableEntities);
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }
        }

        public virtual CommonResponse RemoveFromParent<Parent>(int parentID, Entity entity) where Parent : BaseEntity
        {
            CommonResponse response = new CommonResponse();
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        //repository.ByUserId = LoggedUser.UserID;
                        repository.RemoveFromParent<Parent>(parentID, entity);

                        transaction.Commit();
                    }
                    catch (KnownError error)
                    {
                        transaction.Rollback();
                        return response.Error(error);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return response.Error("ERROR: " + e.ToString());
                    }
                }
            }
            catch (KnownError error)
            {
                return response.Error(error);
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }

            return response.Success();
        }
    }
}
