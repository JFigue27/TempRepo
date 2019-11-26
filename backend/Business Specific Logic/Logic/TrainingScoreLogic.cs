using BusinessSpecificLogic.EF;
using Reusable;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BusinessSpecificLogic.Logic
{
    public interface ITrainingScoreLogic : IDocumentLogic<TrainingScore>
    {
    }

    public class TrainingScoreLogic : DocumentLogic<TrainingScore>, ITrainingScoreLogic
    {

        public TrainingScoreLogic(DbContext context, IDocumentRepository<TrainingScore> repository, LoggedUser LoggedUser) : base(context, repository, LoggedUser)
        {
        }

        protected override IQueryable<TrainingScore> StaticDbQueryForList(IQueryable<TrainingScore> dbQuery)
        {
            var appliesToDC3 = HttpContext.Current.Request["AppliesToDC3"];
            if (!string.IsNullOrEmpty(appliesToDC3) && appliesToDC3 != "undefined" && appliesToDC3 != "null")
            {
                var bAppliesToDC3 = bool.Parse(appliesToDC3);
                if (bAppliesToDC3)
                {
                    dbQuery = dbQuery.Where(e => e.Training.CatCertification.AppliesToDC3 == bAppliesToDC3);
                }
            }

            return dbQuery
                .Include(e => e.Training.CatCertification)
                .Include(e => e.Employee)
                .OrderBy(e => e.Employee.Name);
        }

        //protected override void OnGetSingle(TrainingScore entity)
        //{
        //    var ctx = context as TrainingContext;

        //    StaticDbQueryForList(ctx.TrainingScores).FirstOrDefault(t => t.TrainingScoreKey == entity.TrainingScoreKey);
        //}
    }
}