using BusinessSpecificLogic.EF;
using Reusable;
using System.Data.Entity;
using System.Linq;

namespace BusinessSpecificLogic.Logic
{
    public interface ILevel3Logic : ILogic<Level3>
    {
    }

    public class Level3Logic : Logic<Level3>, ILevel3Logic
    {
        public Level3Logic(DbContext context, IRepository<Level3> repository, LoggedUser LoggedUser) : base(context, repository, LoggedUser)
        {
        }

        protected override IQueryable<Level3> StaticDbQueryForList(IQueryable<Level3> dbQuery)
        {
            return dbQuery
                .Include(e => e.Level2s);
        }

        protected override void onBeforeSaving(Level3 entity, BaseEntity parent = null, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            var ctx = context as TrainingContext;

            if (entity.Level2s != null)
            {
                foreach (var Level2 in entity.Level2s)
                {
                    try
                    {
                        ctx.Level2s.Attach(Level2);
                    }
                    catch
                    {
                        //TODO
                    }
                }
            }

            if (mode == OPERATION_MODE.UPDATE)
            {
                var originalEntity = _GetByID(entity.id);
                ctx.Entry(originalEntity).Collection(e => e.Level2s).Load();

                for (int i = originalEntity.Level2s.Count - 1; i >= 0; i--)
                {
                    var toRemove = originalEntity.Level2s[i];
                    if (!entity.Level2s.Any(e => e.id == toRemove.id))
                    {
                        originalEntity.Level2s.Remove(toRemove);
                    }
                }

                foreach (var toAdd in entity.Level2s)
                {
                    if (!originalEntity.Level2s.Any(e => e.id == toAdd.id))
                    {
                        originalEntity.Level2s.Add(toAdd);
                    }
                }
            }
        }
    }
}