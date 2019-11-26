using BusinessSpecificLogic.EF;
using System.Web.Http;
using BusinessSpecificLogic.Logic;
using Reusable;

namespace ReusableWebAPI.Controllers
{
    [RoutePrefix("api/Training")]
    public class TrainingController : DocumentController<Training>
    {
        public TrainingController(ITrainingLogic logic) : base(logic)
        {
        }

        [HttpGet, Route("GetActiveTrainings/{Level1Key}")]
        public CommonResponse GetActiveTrainings(int Level1Key)
        {
            return (logic as ITrainingLogic).GetActiveTrainings(Level1Key);
        }
    }
}
