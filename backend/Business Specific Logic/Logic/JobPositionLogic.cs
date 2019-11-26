using BusinessSpecificLogic.EF;
using Reusable;
using System.Data.Entity;
using System.Linq;

namespace BusinessSpecificLogic.Logic
{
    public interface IJobPositionLogic : ILogic<JobPosition>
    {
    }

    public class JobPositionLogic : Logic<JobPosition>, IJobPositionLogic
    {
        public JobPositionLogic(DbContext context, IRepository<JobPosition> repository, LoggedUser LoggedUser) : base(context, repository, LoggedUser)
        {
        }

        protected override IQueryable<JobPosition> StaticDbQueryForList(IQueryable<JobPosition> dbQuery)
        {
            return dbQuery
                .Include(e => e.Skills);
        }
        protected override void onBeforeSaving(JobPosition entity, BaseEntity parent = null, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            var ctx = context as TrainingContext;

            if (entity.Skills != null)
            {
                foreach (var skill in entity.Skills)
                {
                    try
                    {
                        skill.JobPositions = null;
                        ctx.Skills.Attach(skill);
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
                var originalEntity = ctx.JobPositions
                    .Include(e => e.Skills)
                    .FirstOrDefault(e => e.JobPositionKey == entity.JobPositionKey);

                //ctx.Entry(originalEntity).Collection(e => e.Skills).Load();

                if (originalEntity == null)
                {
                    throw new KnownError("Original Entity does not exist anymore.");
                }

                for (int i = originalEntity.Skills.Count - 1; i >= 0; i--)
                {
                    var toRemove = originalEntity.Skills[i];
                    if (!entity.Skills.Any(e => e.id == toRemove.id))
                    {
                        originalEntity.Skills.Remove(toRemove);
                    }
                }

                foreach (var toAdd in entity.Skills)
                {
                    if (!originalEntity.Skills.Any(e => e.id == toAdd.id))
                    {
                        originalEntity.Skills.Add(toAdd);
                    }
                }
            }
        }
    }
}
