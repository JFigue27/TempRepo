using BusinessSpecificLogic.EF;
using Reusable;
using System.Data.Entity;
using System.Linq;

namespace BusinessSpecificLogic.Logic
{
    public interface IFormatoDC3Logic : IDocumentLogic<FormatoDC3>
    {
    }

    public class FormatoDC3Logic : DocumentLogic<FormatoDC3>, IFormatoDC3Logic
    {
        public FormatoDC3Logic(DbContext context, IDocumentRepository<FormatoDC3> repository, LoggedUser LoggedUser) : base(context, repository, LoggedUser)
        {
        }

        protected override void OnGetSingle(FormatoDC3 entity)
        {
            var ctx = context as TrainingContext;
            ctx.Employees
                .Include(e => e.Shift)
                .Include(e => e.Level1)
                .Include(e => e.JobPosition)
                .FirstOrDefault(e => e.EmployeeKey == entity.EmployeeKey);
            ctx.TrainingScores
                .Include(e => e.Training)
                .FirstOrDefault(e => e.TrainingScoreKey == entity.TrainingScoreKey);
            ctx.Certifications.FirstOrDefault(e => e.CertificationKey == entity.CertificationKey);
        }

        protected override void onBeforeSaving(FormatoDC3 entity, BaseEntity parent = null, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            var ctx = context as TrainingContext;

            if (entity.Employee != null)
            {
                ctx.Employees.Attach(entity.Employee);
            }

            if (entity.Certification != null)
            {
                ctx.Certifications.Attach(entity.Certification);
            }
        }
    }
}