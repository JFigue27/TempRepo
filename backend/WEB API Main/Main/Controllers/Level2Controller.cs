using BusinessSpecificLogic.EF;
using System.Web.Http;
using BusinessSpecificLogic.Logic;

namespace ReusableWebAPI.Controllers
{
    [RoutePrefix("api/Level2")]
    public class Level2Controller : BaseController<Level2>
    {
        public Level2Controller(ILevel2Logic logic) : base(logic)
        {
        }
    }
}
