using BusinessSpecificLogic.EF;
using System.Web.Http;
using BusinessSpecificLogic.Logic;

namespace ReusableWebAPI.Controllers
{
    [RoutePrefix("api/Level5")]
    public class Level5Controller : BaseController<Level5>
    {
        public Level5Controller(ILevel5Logic logic) : base(logic)
        {
        }
    }
}
