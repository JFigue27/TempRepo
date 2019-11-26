using BusinessSpecificLogic.EF;
using System.Web.Http;
using BusinessSpecificLogic.Logic;

namespace ReusableWebAPI.Controllers
{
    [RoutePrefix("api/Token")]
    public class TokenController : BaseController<Token>
    {
        public TokenController(ITokenLogic logic) : base(logic)
        {
        }
    }
}
