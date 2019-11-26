using BusinessSpecificLogic.EF;
using System.Web.Http;
using BusinessSpecificLogic.Logic;

namespace ReusableWebAPI.Controllers
{
    [RoutePrefix("api/JobPosition")]
    public class JobPositionController : BaseController<JobPosition>
    {
        public JobPositionController(IJobPositionLogic logic) : base(logic)
        {
        }
    }
}
