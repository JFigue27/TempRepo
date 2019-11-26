using BusinessSpecificLogic.EF;
using Reusable;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BusinessSpecificLogic.Logic
{
    public interface ITrainingLogic : IDocumentLogic<Training>
    {
        CommonResponse GetActiveTrainings(int Level1Key);
    }

    public class TrainingLogic : DocumentLogic<Training>, ITrainingLogic
    {
        private readonly TrainingScoreLogic scoreLogic;

        public TrainingLogic(DbContext context, IDocumentRepository<Training> repository, LoggedUser LoggedUser,
            TrainingScoreLogic scoreLogic) : base(context, repository, LoggedUser)
        {
            this.scoreLogic = scoreLogic;
        }

        protected override IQueryable<Training> StaticDbQueryForList(IQueryable<Training> dbQuery)
        {
            return dbQuery
                .Include(e => e.CatCertification)
                .Include(e => e.Supervisor)
                .Include(e => e.Level1)
                .OrderByDescending(e => e.TrainingKey);
            //.Where(e => e.QuickTraining == null);
        }

        protected override bool PopulateForSearch(params Training[] entities)
        {
            AdapterOut(entities);
            return true;
        }

        protected override void AdapterOut(params Training[] entities)
        {
            var ctx = context as TrainingContext;

            var ids = entities.Select(e => e.TrainingKey).ToArray();

            var scores = ctx.TrainingScores
                .Include(s => s.Employee)
                .Where(s => s.sys_active == true)
                .Where(s => ids.Contains(s.TrainingKey)).ToList();

            foreach (var item in entities)
            {
                item.TrainingScores = scores.Where(e => e.TrainingKey == item.TrainingKey).ToList();
                item.IsExpired = item.DateExpiresAt < DateTimeOffset.Now;
                item.AboutToExpire = item.DateExpiresAt < DateTimeOffset.Now.AddDays(15);
                item.CertificationValue = item.CatCertification.Value;
            }
        }

        protected override void OnGetSingle(Training entity)
        {
            var ctx = context as TrainingContext;
            StaticDbQueryForList(ctx.Trainings).FirstOrDefault(e => e.TrainingKey == entity.TrainingKey);
        }

        protected override void onBeforeSaving(Training entity, BaseEntity parent = null, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            var ctx = context as TrainingContext;

            if (string.IsNullOrWhiteSpace(entity.Trainer))
                throw new KnownError("[Trainer] is a required field.");

            if (string.IsNullOrWhiteSpace(entity.InternalExternal))
                throw new KnownError("[Internal or External] is a required field.");

            if (entity.DateStart == null)
                throw new KnownError("[Start Training] is a required field.");

            if (entity.DateEnd == null)
                throw new KnownError("[End Training] is a required field.");

            if (entity.TrainingScores == null || entity.TrainingScores.Count == 0)
                throw new KnownError("At least one Employee should be listed.");


            if (entity.CatCertification != null)
            {
                ctx.Entry(entity.CatCertification).State = EntityState.Unchanged;
            }
        }

        protected override void onAfterSaving(DbContext context, Training entity, BaseEntity parent = null, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            var ctx = context as TrainingContext;

            if (entity.TrainingScores != null && entity.TrainingScores.Count > 0)
            {
                foreach (var score in entity.TrainingScores)
                {
                    //ctx.Employees.Attach(score.Employee);
                    score.EmployeeKey = score.Employee.EmployeeKey;
                    score.Employee = null;

                    score.TrainingKey = entity.id;

                    if (score.TrainingScoreKey > 0)
                    {
                        if (score.EF_State == BaseEntity.EF_EntityState.Deleted)
                        {
                            scoreLogic.repository.Delete(score.id);
                        }
                        else
                        {
                            scoreLogic.Update(score);
                        }
                    }
                    else
                    {
                        if (score.EF_State != BaseEntity.EF_EntityState.Deleted)
                        {
                            scoreLogic.Add(score);
                        }
                    }
                }
            }
        }

        public CommonResponse GetActiveTrainings(int Level1Key)
        {
            CommonResponse response = new CommonResponse();

            var ctx = context as TrainingContext;

            var trainings = ctx.TrainingScores
                 .Include(e => e.Training)
                 .Where(e => e.Training.Level1Key == Level1Key)
                 .Where(e => e.Training.sys_active == true)
                 .GroupBy(e => new
                 {
                     e.EmployeeKey,
                     e.Training.CertificationKey
                 })
                 .Select(e => e.OrderByDescending(c => c.Training.DateExpiresAt).FirstOrDefault())
                 .Select(e => e.Training)
                 .ToList();


            foreach (var training in trainings)
            {
                training.TrainingScores = ctx.TrainingScores.Where(e => e.TrainingKey == training.TrainingKey).ToList();
                training.IsExpired = training.DateExpiresAt < DateTimeOffset.Now;
                training.AboutToExpire = training.DateExpiresAt < DateTimeOffset.Now.AddDays(30);
            }

            return response.Success(trainings);
        }
    }
}