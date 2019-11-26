namespace BusinessSpecificLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vwHeadcount : DbMigration
    {
        public override void Up()
        {
            Sql(@"CREATE VIEW vw_Headcount
                AS
                     SELECT Employee.EmployeeKey, 
                            Employee.Name, 
                            Employee.LastName, 
                            Employee.MotherLastName, 
                            Employee.ClockNumber, 
                            CAST(Employee.HireDate AS DATETIME) AS HireDate, 
                            Employee.CreatedAt, 
                            Employee.TimeIdNumber, 
                            Employee.CURP, 
                            Employee.CheckedoutByKey, 
                            Employee.RevisionMessage, 
                            Employee.STPSPosition, 
                            JobPosition.Value AS JobPosition, 
                            Level1.Value AS Area, 
                            Shift.Value AS Shift, 
                            Employee.PersonalNumber, 
                            Employee.Position, 
                            Employee.AvatarFolder, 
                            Supervisor.Name + ' ' + Supervisor.LastName + ' ' + Supervisor.MotherLastName AS Supervisor
                     FROM Employee
                          LEFT OUTER JOIN Employee AS Supervisor ON Employee.SupervisorKey = Supervisor.EmployeeKey
                          LEFT OUTER JOIN Level1 ON Employee.Level1Key = Level1.Level1Key
                          LEFT OUTER JOIN JobPosition ON Employee.JobPositionKey = JobPosition.JobPositionKey
                          LEFT OUTER JOIN Shift ON Employee.ShiftKey = Shift.ShiftKey
                     WHERE(Employee.sys_active = 1);");
        }
        
        public override void Down()
        {
            Sql("DROP VIEW vw_Headcount");
        }
    }
}
