using Newtonsoft.Json;
using System;
using System.Data.Entity;
using System.Linq;

namespace Reusable
{
    public interface IDocumentLogic<Entity> : ILogic<Entity> where Entity : BaseDocument
    {
        CommonResponse Activate(int id);
        CommonResponse Unlock(Entity document);
        CommonResponse Unlock(int id);
        CommonResponse Lock(Entity document);
        CommonResponse Lock(int id);
        CommonResponse Finalize(Entity document);
        CommonResponse Unfinalize(Entity document);
        CommonResponse Checkout(int id);
        CommonResponse Checkin(Entity document);
        CommonResponse CancelCheckout(int id);
        CommonResponse MakeRevision(Entity document);
    }

    public abstract class DocumentLogic<Document> : Logic<Document>, IDocumentLogic<Document> where Document : BaseDocument, new()
    {
        public new IDocumentRepository<Document> repository;
        protected RevisionLogic revisionLogic;

        public DocumentLogic(DbContext context, IDocumentRepository<Document> repository, LoggedUser LoggedUser) : base(context, repository, LoggedUser)
        {
            this.repository = repository;
        }

        protected virtual void OnBeforeRevision(Document clone) { }

        void CommonDocumentValidations(Document document)
        {
            var originalDocument = repository.GetByID(document.id);
            if (originalDocument == null || originalDocument.sys_active == false)
            {
                throw new KnownError("Document does not exist.");
            }

            //Validate User is Adminsitrator or the one did Checkout
            if (originalDocument.CheckedoutByKey > 0)
            {
                if (LoggedUser.Role != "Administrator")
                {
                    if (LoggedUser.UserID == null
                        || LoggedUser.UserID != originalDocument.CheckedoutByKey)
                    {
                        throw new KnownError("Only User who Checked Out can Check In.");
                    }
                }
            }
        }

        public override void Add(Document document)
        {
            //CommonDocumentValidations(document);

            base.Add(document);
        }

        public override void Update(Document document)
        {
            CommonDocumentValidations(document);

            base.Update(document);
        }

        public virtual CommonResponse Activate(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        //repository.ByUserId = LoggedUser.UserID;
                        repository.Activate(id);

                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return response.Error("ERROR: " + e.ToString());
                    }
                }

            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }
            return response.Success(id);
        }

        public virtual CommonResponse Lock(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        //repository.ByUserId = LoggedUser.UserID;
                        repository.Lock(id);

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
            return response.Success(id);
        }

        public virtual CommonResponse Lock(Document entity)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        //repository.ByUserId = LoggedUser.UserID;
                        repository.Lock(entity);

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

        public virtual CommonResponse Unlock(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        //repository.ByUserId = LoggedUser.UserID;
                        repository.Unlock(id);

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
            return response.Success(id);
        }

        public virtual CommonResponse Unlock(Document entity)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        //repository.ByUserId = LoggedUser.UserID;
                        repository.Unlock(entity);

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

        public virtual CommonResponse Finalize(Document entity)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        //repository.ByUserId = LoggedUser.UserID;

                        onFinalize(entity);

                        repository.Finalize(entity);

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

        public virtual CommonResponse Unfinalize(Document entity)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        //repository.ByUserId = LoggedUser.UserID;

                        onUnfinalize(entity);

                        repository.Unfinalize(entity);

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

        public CommonResponse MakeRevision(Document document)
        {
            CommonResponse response = new CommonResponse();

            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Update(document);

                        _makeRevision(document);

                        transaction.Commit();
                        AdapterOut(document);

                        return response.Success(document);
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
        }

        public CommonResponse Checkout(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                Document document = repository.GetByID(id);

                if (document.CheckedoutByKey == null)
                {
                    document.CheckedoutByKey = LoggedUser.UserID;
                    context.SaveChanges();
                }
                else if (document.CheckedoutByKey > 0 && LoggedUser.UserID != document.CheckedoutByKey)
                {
                    document.CheckedoutBy = context.Set<User>().FirstOrDefault(u => u.UserKey == document.CheckedoutByKey);
                    throw new KnownError("Error: This document is already Checked Out by: " + document.CheckedoutBy?.Value);
                }

                return response.Success(_GetByID(id));
            }
            catch (KnownError ke)
            {
                return response.Error(ke);
            }
            catch (Exception ex)
            {
                return response.Error(ex.ToString());
            }
        }

        public CommonResponse CancelCheckout(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                Document document = repository.GetByID(id);

                if (document.CheckedoutByKey > 0 && LoggedUser.UserID != document.CheckedoutByKey && LoggedUser.Role != "Administrator")
                {
                    document.CheckedoutBy = context.Set<User>().FirstOrDefault(u => u.UserKey == document.CheckedoutByKey);
                    throw new KnownError(@"Error: Only User who Checked Out can ""Cancel Checked Out"": "
                        + document.CheckedoutBy?.Value);
                }

                document.CheckedoutByKey = null;
                context.SaveChanges();

                return response.Success(_GetByID(id));
            }
            catch (KnownError ke)
            {
                return response.Error(ke);
            }
            catch (Exception ex)
            {
                return response.Error(ex.ToString());
            }
        }

        public CommonResponse Checkin(Document document)
        {
            CommonResponse response = new CommonResponse();

            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        document.CheckedoutByKey = null;
                        Update(document);

                        _makeRevision(document);

                        transaction.Commit();
                        AdapterOut(document);
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

            return response.Success(document);
        }

        public void _makeRevision(Document document)
        {
            revisionLogic = new RevisionLogic(context, new DocumentRepository<Revision>(context, LoggedUser), LoggedUser);

            //Revision itself:
            Document clone = (Document)document.Clone();
            clone.Revisions = null;

            OnBeforeRevision(clone);

            Revision revision = new Revision
            {
                ForeignType = document.AAA_EntityName,
                ForeignKey = document.id,
                Value = JsonConvert.SerializeObject(clone),
                RevisionMessage = document.RevisionMessage,
                CreatedAt = DateTimeOffset.Now
            };

            revisionLogic.Add(revision);
        }
    }
}
