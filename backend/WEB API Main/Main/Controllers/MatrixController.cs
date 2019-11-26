using BusinessSpecificLogic.EF;
using System.Web.Http;
using BusinessSpecificLogic.Logic;

namespace ReusableWebAPI.Controllers
{
    [RoutePrefix("api/Matrix")]
    public class MatrixController : DocumentController<Employee>
    {
        public MatrixController(IEmployeeLogic logic) : base(logic)
        {
        }

    }
}
