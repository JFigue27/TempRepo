using System.Web.Http;
using BusinessSpecificLogic.Logic;
using Reusable.Workflows;

namespace ReusableWebAPI.Controllers
{
    [RoutePrefix("api/Workflow")]
    public class WorkflowController : BaseController<Workflow>
    {
        public WorkflowController(IWorkflowLogic logic) : base(logic)
        {
        }
    }
}
