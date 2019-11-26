using Reusable;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessSpecificLogic.EF
{
    [Table("TrainingScore")]
    public class TrainingScore : BaseDocument
    {
        public TrainingScore()
        {
            ///Start:Generated:Constructor<<<
            ///End:Generated:Constructor<<<            
        }

        [Key]
        public int TrainingScoreKey { get; set; }

        public override int id { get { return TrainingScoreKey; } set { TrainingScoreKey = value; } }

        ///Start:Generated:Properties<<<
        public int Score { get; set; }
        public string Notes { get; set; }
        public string ILUO { get; set; }
        public int TrainingKey { get; set; }
        [ForeignKey("TrainingKey")]
        public Training Training { get; set; }
        public int EmployeeKey { get; set; }
        [ForeignKey("EmployeeKey")]
        public Employee Employee { get; set; }
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
        public int? SupervisorKey { get; set; }
        [ForeignKey("SupervisorKey")]
        public Employee Supervisor { get; set; }
        ///End:Generated:Properties<<<
    }
}