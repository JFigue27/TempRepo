using BusinessSpecificLogic.EF;
using System.Web.Http;
using BusinessSpecificLogic.Logic;

namespace ReusableWebAPI.Controllers
{
    [RoutePrefix("api/Shift")]
    public class ShiftController : BaseController<Shift>
    {
        public ShiftController(IShiftLogic logic) : base(logic)
        {
        }
    }
}
