using System.Web.Http;
using BusinessSpecificLogic.Logic;
using Reusable;

namespace ReusableWebAPI.Controllers
{
    [RoutePrefix("api/Application")]
    public class ApplicationController : BaseController<Application>
    {
        public ApplicationController(IApplicationLogic logic) : base(logic)
        {
        }
    }
}
