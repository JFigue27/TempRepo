using BusinessSpecificLogic.EF;
using Reusable;
using System.Data.Entity;
using System.Linq;

namespace BusinessSpecificLogic.Logic
{
    public interface ILevel4Logic : ILogic<Level4>
    {
    }

    public class Level4Logic : Logic<Level4>, ILevel4Logic
    {
        public Level4Logic(DbContext context, IRepository<Level4> repository, LoggedUser LoggedUser) : base(context, repository, LoggedUser)
        {
        }

        protected override IQueryable<Level4> StaticDbQueryForList(IQueryable<Level4> dbQuery)
        {
            return dbQuery
                .Include(e => e.Level3s);
        }

        protected override void onBeforeSaving(Level4 entity, BaseEntity parent = null, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            var ctx = context as TrainingContext;

            if (entity.Level3s != null)
            {
                foreach (var Leve3 in entity.Level3s)
                {
                    try
                    {
                        ctx.Level3s.Attach(Leve3);
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
                ctx.Entry(originalEntity).Collection(e => e.Level3s).Load();

                for (int i = originalEntity.Level3s.Count - 1; i >= 0; i--)
                {
                    var toRemove = originalEntity.Level3s[i];
                    if (!entity.Level3s.Any(e => e.id == toRemove.id))
                    {
                        originalEntity.Level3s.Remove(toRemove);
                    }
                }

                foreach (var toAdd in entity.Level3s)
                {
                    if (!originalEntity.Level3s.Any(e => e.id == toAdd.id))
                    {
                        originalEntity.Level3s.Add(toAdd);
                    }
                }
            }
        }
    }
}