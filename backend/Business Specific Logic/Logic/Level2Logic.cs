using BusinessSpecificLogic.EF;
using Reusable;
using System.Data.Entity;
using System.Linq;

namespace BusinessSpecificLogic.Logic
{
    public interface ILevel2Logic : ILogic<Level2>
    {
    }

    public class Level2Logic : Logic<Level2>, ILevel2Logic
    {
        public Level2Logic(DbContext context, IRepository<Level2> repository, LoggedUser LoggedUser) : base(context, repository, LoggedUser)
        {
        }

        protected override IQueryable<Level2> StaticDbQueryForList(IQueryable<Level2> dbQuery)
        {
            return dbQuery
                .Include(e => e.Level1s);
        }

        protected override void onBeforeSaving(Level2 entity, BaseEntity parent = null, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            var ctx = context as TrainingContext;

            if (entity.Level1s != null)
            {
                foreach (var level1 in entity.Level1s)
                {
                    try
                    {
                        ctx.Level1s.Attach(level1);
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