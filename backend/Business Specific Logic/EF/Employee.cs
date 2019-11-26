using Reusable;
using Reusable.Attachments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessSpecificLogic.EF
{
    [Table("Employee")]
    public class Employee : BaseDocument, IAvatar
    {
        public Employee()
        {
            ///Start:Generated:Constructor<<<
            ///End:Generated:Constructor<<<
        }

        [Key]
        public int EmployeeKey { get; set; }

        public override int id { get { return EmployeeKey; } set { EmployeeKey = value; } }

        ///Start:Generated:Properties<<<
        public string Name { get; set; }
        public string LastName { get; set; }
        public string MotherLastName { get; set; }
        public string ClockNumber { get; set; }
        public string TimeIdNumber { get; set; }
        public string PersonalNumber { get; set; }
        public DateTimeOffset? HireDate { get; set; }
        public string Position { get; set; }
        public string STPSPosition { get; set; }
        public string CURP { get; set; }
        public List<Training> Trainings { get; set; }
        public int? Level1Key { get; set; }
        [ForeignKey("Level1Key")]
        public Level1 Level1 { get; set; }
        public int? Level2Key { get; set; }
        [ForeignKey("Level2Key")]
        public Level2 Level2 { get; set; }
        public int? Level3Key { get; set; }
        [ForeignKey("Level3Key")]
        public Level3 Level3 { get; set; }
        public int? Level4Key { get; set; }
        [ForeignKey("Level4Key")]
        public Level4 Level4 { get; set; }
        public int? Level5Key { get; set; }
        [ForeignKey("Level5Key")]
        public Level5 Level5 { get; set; }
        public int? ShiftKey { get; set; }
        [ForeignKey("ShiftKey")]
        public Shift Shift { get; set; }
        public int? JobPositionKey { get; set; }
        [ForeignKey("JobPositionKey")]
        public JobPosition JobPosition { get; set; }
        public int? SupervisorKey { get; set; }
        [ForeignKey("SupervisorKey")]
        public Employee Supervisor { get; set; }
        ///End:Generated:Properties<<<

        [NotMapped]
        public ICollection<TrainingScore> TrainingScores { get; set; }

        public string AvatarFolder { get; set; }
        public List<Avatar> AvatarList { get; set; }

        public List<Skill> Skills { get; set; }
    }
}
