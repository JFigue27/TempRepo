using System.Web.Http;
using BusinessSpecificLogic.Logic;
using Reusable;
using System;
using Newtonsoft.Json;

namespace ReusableWebAPI.Controllers
{
    [RoutePrefix("api/Track")]
    public class TrackController : BaseController<Track>
    {
        public TrackController(ITrackLogic logic) : base(logic)
        {
        }

        [HttpPost, Route("assignResponsible/{iTrackKey}/{iUserKey}")]
        public CommonResponse AssignResponsible(int iTrackKey, int iUserKey)
        {
            return (logic as ITrackLogic).AssignResponsible(iTrackKey, iUserKey);
        }
    }
}
