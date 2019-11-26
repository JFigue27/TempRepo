using BusinessSpecificLogic.EF;
using Reusable;
using System.Data.Entity;

namespace BusinessSpecificLogic.Logic
{
    public interface IRoleLogic : ILogic<Role>
    {
    }

    public class RoleLogic : Logic<Role>, IRoleLogic
    {
        public RoleLogic(DbContext context, IRepository<Role> repository, LoggedUser LoggedUser) : base(context, repository, LoggedUser)
        {
        }
    }
}
