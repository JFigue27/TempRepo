using BusinessSpecificLogic.EF;
using System.Web.Http;
using BusinessSpecificLogic.Logic;

namespace ReusableWebAPI.Controllers
{
    [RoutePrefix("api/Employee")]
    public class EmployeeController : DocumentController<Employee>
    {
        public EmployeeController(IEmployeeLogic logic) : base(logic)
        {
        }
    }
}
