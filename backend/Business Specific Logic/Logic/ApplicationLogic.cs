using Reusable;
using System.Data.Entity;
using System.Linq;

namespace BusinessSpecificLogic.Logic
{
    public interface IApplicationLogic : ILogic<Application>
    {
    }

    public class ApplicationLogic : DocumentLogic<Application>, IApplicationLogic
    {
        public ApplicationLogic(DbContext context, IDocumentRepository<Application> repository, LoggedUser LoggedUser) : base(context, repository, LoggedUser)
        {
        }

        protected override IQueryable<Application> StaticDbQueryForList(IQueryable<Application> dbQuery)
        {
            return dbQuery
                .Include(e => e.Roles)
                .OrderBy(e => e.Name);
        }
    }
}
