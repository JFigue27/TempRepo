using BusinessSpecificLogic.EF;
using System.Web.Http;
using BusinessSpecificLogic.Logic;

namespace ReusableWebAPI.Controllers
{
    [RoutePrefix("api/Skill")]
    public class SkillController : BaseController<Skill>
    {
        public SkillController(ISkillLogic logic) : base(logic)
        {
        }
    }
}
