namespace BusinessSpecificLogic.Migrations
{
    using BusinessSpecificLogic.EF;
    using Reusable.Workflows;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<TrainingContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(TrainingContext context)
        {
            #region Workflows

            #endregion

            context.JobPositions.AddOrUpdate(new JobPosition()
            {
                JobPositionKey = 1,
                Value = "Supervisor"
            });

        }
    }
}