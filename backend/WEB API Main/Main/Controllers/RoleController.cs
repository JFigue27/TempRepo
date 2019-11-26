using System.Web.Http;
using BusinessSpecificLogic.Logic;
using Reusable;

namespace ReusableWebAPI.Controllers
{
    [RoutePrefix("api/Role")]
    public class RoleController : BaseController<Role>
    {
        public RoleController(IRoleLogic logic) : base(logic)
        {
        }
    }
}
