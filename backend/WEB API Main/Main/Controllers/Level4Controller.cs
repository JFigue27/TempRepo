using BusinessSpecificLogic.EF;
using System.Web.Http;
using BusinessSpecificLogic.Logic;

namespace ReusableWebAPI.Controllers
{
    [RoutePrefix("api/Level4")]
    public class Level4Controller : BaseController<Level4>
    {
        public Level4Controller(ILevel4Logic logic) : base(logic)
        {
        }
    }
}
