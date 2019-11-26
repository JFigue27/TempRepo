using Reusable;
using Reusable.Workflows;
using System.Data.Entity;

namespace BusinessSpecificLogic.Logic
{
    public interface IStepLogic : ILogic<Step>
    {
    }

    public class StepLogic : Logic<Step>, IStepLogic
    {
        public StepLogic(DbContext context, IRepository<Step> repository, LoggedUser LoggedUser) : base(context, repository, LoggedUser)
        {
        }
    }
}