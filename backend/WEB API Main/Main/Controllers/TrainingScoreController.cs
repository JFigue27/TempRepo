using BusinessSpecificLogic.EF;
using System.Web.Http;
using BusinessSpecificLogic.Logic;

namespace ReusableWebAPI.Controllers
{
    [RoutePrefix("api/TrainingScore")]
    public class TrainingScoreController : DocumentController<TrainingScore>
    {
        public TrainingScoreController(ITrainingScoreLogic logic) : base(logic)
        {
        }
    }
}
