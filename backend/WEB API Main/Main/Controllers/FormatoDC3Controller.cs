using BusinessSpecificLogic.EF;
using System.Web.Http;
using BusinessSpecificLogic.Logic;

namespace ReusableWebAPI.Controllers
{
    [RoutePrefix("api/FormatoDC3")]
    public class FormatoDC3Controller : DocumentController<FormatoDC3>
    {
        public FormatoDC3Controller(IFormatoDC3Logic logic) : base(logic)
        {
        }
    }
}
