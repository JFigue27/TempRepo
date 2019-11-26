using BusinessSpecificLogic.EF;
using System.Web.Http;
using BusinessSpecificLogic.Logic;

namespace ReusableWebAPI.Controllers
{
    [RoutePrefix("api/Level1")]
    public class Level1Controller : BaseController<Level1>
    {
        public Level1Controller(ILevel1Logic logic) : base(logic)
        {
        }
    }
}
