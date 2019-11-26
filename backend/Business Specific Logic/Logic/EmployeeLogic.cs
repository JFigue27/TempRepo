using BusinessSpecificLogic.EF;
using Reusable;
using Reusable.Attachments;
using System.Data.Entity;
using System.Linq;

namespace BusinessSpecificLogic.Logic
{
    public interface IEmployeeLogic : IDocumentLogic<Employee>
    {
    }

    public class EmployeeLogic : DocumentLogic<Employee>, IEmployeeLogic
    {
        protected readonly TrainingScoreLogic trainingScoreLogic;

        public EmployeeLogic(DbContext context, IDocumentRepository<Employee> repository, LoggedUser LoggedUser,
            TrainingScoreLogic trainingScoreLogic) : base(context, repository, LoggedUser)
        {
            this.trainingScoreLogic = trainingScoreLogic;
        }

        protected override IQueryable<Employee> StaticDbQueryForList(IQueryable<Employee> dbQuery) // Select * From Employee
        {
            return dbQuery
                .Include(e => e.JobPosition) // Left Join JobPosition ON JobPosition.JobPositionKey = Employe.JobPositionKey
                .Include(e => e.Shift)
                .Include(e => e.Skills)
                .Include(e => e.Level1)
                .Include(e => e.Level2)
                .Include(e => e.Level3)
                .Include(e => e.Level4)
                .Include(e => e.Level5);
        }

        protected override void OnGetSingle(Employee entity) // 
        {
            var ctx = context as TrainingContext;

            StaticDbQueryForList(ctx.Employees)
                .FirstOrDefault(e => e.EmployeeKey == entity.EmployeeKey);

            entity.TrainingScores = trainingScoreLogic._GetListWhere(t => t.Training.CatCertification.Value, t => t.EmployeeKey == entity.EmployeeKey, t => t.sys_active == true).ToList();
        }

        protected override void onBeforeSaving(Employee entity, BaseEntity parent = null, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            var ctx = context as TrainingContext;

            if (entity.Skills != null)
            {
                foreach (var skill in entity.Skills)
                {
                    try
                    {
                        if (skill.id > 0)
                        {
                            skill.Employees = null;
                            ctx.Skills.Attach(skill);
                        } else
                        {
                            ctx.Skills.Add(skill);
                        }
                    }
                    catch
                    {
                        //TODO
                    }
                }
            }

            if (mode == OPERATION_MODE.UPDATE)
            {
                //var originalEntity = _GetByID(entity.id);
                var originalEntity = ctx.Employees
                    .Include(e => e.Skills)
                    .FirstOrDefault(e => e.EmployeeKey == entity.EmployeeKey);

                //ctx.Entry(originalEntity).Collection(e => e.Skills).Load();

                if (originalEntity == null)
                {
                    throw new KnownError("Original Entity no longer exists.");
                }

                for (int i = originalEntity.Skills.Count - 1; i >= 0; i--)
                {
                    var toRemove = originalEntity.Skills[i];
                    if (!entity.Skills.Any(e => e.id == toRemove.id))
                    {
                        originalEntity.Skills.Remove(toRemove);
                    }
                }

                foreach (var toAdd in entity.Skills)
                {
                    if (!originalEntity.Skills.Any(e => e.id == toAdd.id))
                    {
                        originalEntity.Skills.Add(toAdd);
                    }
                }
            }
        }

        protected override void AdapterOut(params Employee[] entities)
        {
            foreach (var item in entities)
            {
                item.AvatarList = AttachmentsIO.getAvatarsFromFolder(item.AvatarFolder, "Avatar");

                if (item.TrainingScores != null)
                {
                    if (item.Level1Key > 0) {
                        for (int i = item.TrainingScores.Count - 1; i >= 0; i--)
                        {
                            var score = item.TrainingScores.ElementAt(i);
                            if (score.Training.Level1Key != item.Level1Key)
                            {
                                item.TrainingScores.Remove(score);
                            }
                        }

                    }

                    item.TrainingScores = (from e in item.TrainingScores
                                          group e by e.Training.CertificationKey
                                          into groups
                                          select groups.OrderByDescending(c => c.Training.DateExpiresAt).First()).ToList();
                 }
            }
        }
    }
}