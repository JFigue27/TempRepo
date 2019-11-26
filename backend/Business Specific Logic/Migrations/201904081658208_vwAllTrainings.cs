namespace BusinessSpecificLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vwAllTrainings : DbMigration
    {
        public override void Up()
        {
            Sql(@"CREATE VIEW vw_AllTrainings
            AS
                 SELECT vw_Headcount.EmployeeKey, 
                        vw_Headcount.Name, 
                        vw_Headcount.LastName, 
                        vw_Headcount.MotherLastName, 
                        vw_Headcount.ClockNumber, 
                        vw_Headcount.HireDate, 
                        vw_Headcount.CreatedAt, 
                        vw_Headcount.TimeIdNumber, 
                        vw_Headcount.CURP, 
                        vw_Headcount.CheckedoutByKey, 
                        vw_Headcount.RevisionMessage, 
                        vw_Headcount.STPSPosition, 
                        vw_Headcount.JobPosition, 
                        vw_Headcount.Area, 
                        vw_Headcount.Shift, 
                        vw_Headcount.PersonalNumber, 
                        vw_Headcount.Position, 
                        vw_Headcount.AvatarFolder, 
                        vw_Headcount.Supervisor, 
                        TrainingScore.TrainingScoreKey, 
                        CAST(TrainingScore.CreatedAt AS DATETIME) AS ScoreCreatedAt, 
                        TrainingScore.Score, 
                        TrainingScore.Notes AS ScoreNotes, 
                        Training.TrainingKey, 
                        CAST(Training.CreatedAt AS DATETIME) AS TrainingCreatedAt, 
                        CAST(Training.DateProgrammed AS DATETIME) AS DateProgrammed, 
                        CAST(Training.DateStart AS DATETIME) AS DateStart, 
                        CAST(Training.DateEnd AS DATETIME) AS DateEnd, 
                        CAST(Training.DateCertification AS DATETIME) AS DateCertification, 
                        CAST(Training.DateExpiresAt AS DATETIME) AS DateExpiresAt, 
                        Training.Trainer, 
                        Training.InternalExternal, 
                        Training.Notes AS TrainingNotes, 
                        Training.QuickTraining, 
                        Training.DurationInHours, 
                        Certification.CertificationKey, 
                        Certification.Value AS Certification, 
                        Certification.AppliesToDC3, 
                        Certification.LifecycleInMonths, 
                        Certification.ThematicArea, 
                        Certification.VisibleToCard, 
                        Certification.ThematicAreaKey
                 FROM TrainingScore
                      INNER JOIN Training ON TrainingScore.TrainingKey = Training.TrainingKey
                      INNER JOIN Certification ON Training.CertificationKey = Certification.CertificationKey
                      RIGHT OUTER JOIN vw_Headcount ON TrainingScore.EmployeeKey = vw_Headcount.EmployeeKey
                 WHERE(TrainingScore.sys_active = 1)
                      AND (Training.sys_active = 1);");
        }
        
        public override void Down()
        {
            Sql("DROP VIEW vw_AllTrainings");
        }
    }
}
