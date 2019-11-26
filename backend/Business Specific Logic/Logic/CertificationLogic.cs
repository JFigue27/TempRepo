using BusinessSpecificLogic.EF;
using Reusable;
using System.Data.Entity;
using System.Linq;

namespace BusinessSpecificLogic.Logic
{
    public interface ICertificationLogic : ILogic<Certification>
    {
    }

    public class CertificationLogic : Logic<Certification>, ICertificationLogic
    {
        public CertificationLogic(DbContext context, IRepository<Certification> repository, LoggedUser LoggedUser) : base(context, repository, LoggedUser)
        {
        }

        protected override IQueryable<Certification> StaticDbQueryForList(IQueryable<Certification> dbQuery)
        {
            return dbQuery
                .Include(e => e.Level1s);
        }

        protected override void onBeforeSaving(Certification entity, BaseEntity parent = null, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            var ctx = context as TrainingContext;

            if (entity.Level1s != null)
            {
                foreach (var productionLine in entity.Level1s)
                {
                    ctx.Level1s.Attach(productionLine);
                }
            }

            if (mode == OPERATION_MODE.UPDATE)
            {
                var originalEntity = _GetByID(entity.id);
                ctx.Entry(originalEntity).Collection(e => e.Level1s).Load();

                for (int i = originalEntity.Level1s.Count - 1; i >= 0; i--)
                {
                    var toRemove = originalEntity.Level1s[i];
                    if (!entity.Level1s.Any(e => e.id == toRemove.id))
                    {
                        originalEntity.Level1s.Remove(toRemove);
                    }
                }

                foreach (var toAdd in entity.Level1s)
                {
                    if (!originalEntity.Level1s.Any(e => e.id == toAdd.id))
                    {
                        originalEntity.Level1s.Add(toAdd);
                    }
                }
            }
        }
    }
}
