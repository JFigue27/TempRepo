using BusinessSpecificLogic.EF;
using Reusable;
using System.Data.Entity;
using System.Linq;

namespace BusinessSpecificLogic.Logic
{
    public interface ILevel1Logic : ILogic<Level1>
    {
    }

    public class Level1Logic : Logic<Level1>, ILevel1Logic
    {
        public Level1Logic(DbContext context, IRepository<Level1> repository, LoggedUser LoggedUser) : base(context, repository, LoggedUser)
        {
        }

        protected override IQueryable<Level1> StaticDbQueryForList(IQueryable<Level1> dbQuery)
        {
            return dbQuery
                .Include(e => e.Certifications);
        }

        protected override void onBeforeSaving(Level1 entity, BaseEntity parent = null, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            var ctx = context as TrainingContext;

            if (entity.Certifications != null)
            {
                foreach (var certification in entity.Certifications)
                {
                    try
                    {
                        certification.Level1s = null;
                        ctx.Certifications.Attach(certification);
                    }
                    catch
                    {
                        //TODO
                    }
                }
            }

            if (mode == OPERATION_MODE.UPDATE)
            {
                //var originalEntity = _GetByID(entity.id);
                var originalEntity = ctx.Level1s
                    .Include(e => e.Certifications)
                    .FirstOrDefault(e => e.Level1Key == entity.Level1Key);

                //ctx.Entry(originalEntity).Collection(e => e.Certifications).Load();

                if (originalEntity == null)
                {
                    throw new KnownError("Original Entity does not exist anymore.");
                }

                for (int i = originalEntity.Certifications.Count - 1; i >= 0; i--)
                {
                    var toRemove = originalEntity.Certifications[i];
                    if (!entity.Certifications.Any(e => e.id == toRemove.id))
                    {
                        originalEntity.Certifications.Remove(toRemove);
                    }
                }

                foreach (var toAdd in entity.Certifications)
                {
                    if (!originalEntity.Certifications.Any(e => e.id == toAdd.id))
                    {
                        originalEntity.Certifications.Add(toAdd);
                    }
                }
            }
        }
    }
}
