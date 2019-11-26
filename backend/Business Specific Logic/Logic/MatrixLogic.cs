using BusinessSpecificLogic.EF;
using Reusable;
using System.Data.Entity;
using System.Linq;

namespace BusinessSpecificLogic.Logic
{
    public interface IMatrixLogic : IEmployeeLogic
    {
    }

    public class MatrixLogic : EmployeeLogic, IMatrixLogic
    {
        public MatrixLogic(DbContext context, IDocumentRepository<Employee> repository, LoggedUser LoggedUser, TrainingScoreLogic trainingScoreLogic) : base(context, repository, LoggedUser, trainingScoreLogic)
        {
        }

        protected override void AdapterOut(params Employee[] entities)
        {
            foreach (var item in entities)
            {
                item.TrainingScores = trainingScoreLogic._GetListWhere(t => t.Training.CatCertification.Value, t => t.EmployeeKey == item.EmployeeKey).ToList();
            }
            base.AdapterOut(entities);
        }
    }
}