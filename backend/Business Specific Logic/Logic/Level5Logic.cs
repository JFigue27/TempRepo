using BusinessSpecificLogic.EF;
using Reusable;
using System.Data.Entity;
using System.Linq;

namespace BusinessSpecificLogic.Logic
{
    public interface ILevel5Logic : ILogic<Level5>
    {
    }

    public class Level5Logic : Logic<Level5>, ILevel5Logic
    {
        public Level5Logic(DbContext context, IRepository<Level5> repository, LoggedUser LoggedUser) : base(context, repository, LoggedUser)
        {
        }

        protected override IQueryable<Level5> StaticDbQueryForList(IQueryable<Level5> dbQuery)
        {
            return dbQuery
                .Include(e => e.Level4s);
        }

        protected override void onBeforeSaving(Level5 entity, BaseEntity parent = null, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            var ctx = context as TrainingContext;

            if (entity.Level4s != null)
            {
                foreach (var Level4 in entity.Level4s)
                {
                    try
                    {
                        ctx.Level4s.Attach(Level4);
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
                ctx.Entry(originalEntity).Collection(e => e.Level4s).Load();

                for (int i = originalEntity.Level4s.Count - 1; i >= 0; i--)
                {
                    var toRemove = originalEntity.Level4s[i];
                    if (!entity.Level4s.Any(e => e.id == toRemove.id))
                    {
                        originalEntity.Level4s.Remove(toRemove);
                    }
                }

                foreach (var toAdd in entity.Level4s)
                {
                    if (!originalEntity.Level4s.Any(e => e.id == toAdd.id))
                    {
                        originalEntity.Level4s.Add(toAdd);
                    }
                }
            }
        }
    }
}