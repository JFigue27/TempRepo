using Reusable;
using Reusable.Workflows;
using System.Data.Entity;

namespace BusinessSpecificLogic.Logic
{
    public interface IStepOperationLogic : ILogic<StepOperation>
    {
    }

    public class StepOperationLogic : Logic<StepOperation>, IStepOperationLogic
    {
        public StepOperationLogic(DbContext context, IRepository<StepOperation> repository, LoggedUser LoggedUser) : base(context, repository, LoggedUser)
        {
        }
    }
}
