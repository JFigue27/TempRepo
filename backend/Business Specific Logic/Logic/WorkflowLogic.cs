using Reusable;
using Reusable.Workflows;
using System.Data.Entity;
using BusinessSpecificLogic.EF;
using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace BusinessSpecificLogic.Logic
{
    public interface IWorkflowLogic : ILogic<Workflow>
    {
        List<StepOperation> CreateStepsFromTemplate(string sWorkflowType, int ForeignParentKey);
    }

    public class WorkflowLogic : Logic<Workflow>, IWorkflowLogic
    {
        private readonly IStepLogic stepLogic;

        public WorkflowLogic(DbContext context, IRepository<Workflow> repository, LoggedUser LoggedUser,
            IStepLogic stepLogic) : base(context, repository, LoggedUser)
        {
            this.stepLogic = stepLogic;
        }

        protected override void AdapterOut(params Workflow[] entities)
        {
            var ctx = context as TrainingContext;

            foreach (var item in entities)
            {
                item.Steps = ctx.Steps.Where(s => s.WorkflowKey == item.WorkflowKey).ToList();
                foreach (var step in item.Steps)
                {
                    stepLogic.FillRecursively<Step>(step);
                }
            }
        }

        public List<StepOperation> CreateStepsFromTemplate(string sWorkflowType, int ForeignParentKey)
        {
            var ctx = context as TrainingContext;

            //repository.ByUserId = LoggedUser.UserID;

            var workflowResponse = GetSingleWhere(w => w.Name == sWorkflowType);
            if (workflowResponse.ErrorThrown)
            {
                throw new KnownError(workflowResponse.ResponseDescription);
            }

            var workflow = (Workflow)workflowResponse.Result;

            if (workflow == null)
            {
                throw new KnownError("Did not find Workflow with name: " + sWorkflowType);
            }

            List<StepOperation> stepsCopy = new List<StepOperation>();
            foreach (var item in workflow.Steps)
            {
                StepOperation newStep = DuplicateRecursively(item, null, workflow.id, ForeignParentKey);
                stepsCopy.Add(newStep);
            }

            return stepsCopy;
        }

        public StepOperation DuplicateRecursively(Step step, int? iParentKey, int iWorkflowTypeKey, int? ForeignParentKey)
        {
            StepOperation stepCopy = new StepOperation()
            {
                WorkflowKey = iWorkflowTypeKey,
                ParentKey = iParentKey,
                Name = step.Name,
                ForeignParentKey = ForeignParentKey,
                ProcessType = step.ProcessType
            };

            string sqlInsertStepOperation = @"INSERT INTO WorkflowStepOperation
                                           ([Name]
                                           ,[WorkflowKey]
                                           ,[ParentKey]
                                           ,[ForeignParentKey]
                                           ,[ProcessType])
                                     VALUES
                                           (@Name
                                           ,@WorkflowKey
                                           ,@ParentKey
                                           ,@ForeignParentKey
                                           ,@ProcessType);
                                    SELECT SCOPE_IDENTITY();";

            stepCopy.StepKey = context.Database.Connection.QueryFirst<int>(sqlInsertStepOperation, stepCopy, context.Database.CurrentTransaction.UnderlyingTransaction);

            foreach (var node in step.nodes)
            {
                StepOperation newStep = DuplicateRecursively((Step)node, stepCopy.id, iWorkflowTypeKey, null);
                stepCopy.nodes.Add(newStep);
            }

            return stepCopy;
        }

    }
}