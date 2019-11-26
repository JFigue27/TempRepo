using BusinessSpecificLogic.EF;
using System.Web.Http;
using BusinessSpecificLogic.Logic;

namespace ReusableWebAPI.Controllers
{
    [RoutePrefix("api/Level3")]
    public class Level3Controller : BaseController<Level3>
    {
        public Level3Controller(ILevel3Logic logic) : base(logic)
        {
        }
    }
}
