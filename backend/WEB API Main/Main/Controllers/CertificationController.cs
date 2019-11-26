using BusinessSpecificLogic.EF;
using System.Web.Http;
using BusinessSpecificLogic.Logic;

namespace ReusableWebAPI.Controllers
{
    [RoutePrefix("api/Certification")]
    public class CertificationController : BaseController<Certification>
    {
        public CertificationController(ICertificationLogic logic) : base(logic)
        {
        }
    }
}
