using Reusable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessSpecificLogic.EF
{
    [Table("Training")]
    public class Training : BaseDocument
    {
        public Training()
        {
            ///Start:Generated:Constructor<<<
            ///End:Generated:Constructor<<<


        }

        [Key]
        public int TrainingKey { get; set; }

        public override int id { get { return TrainingKey; } set { TrainingKey = value; } }

        ///Start:Generated:Properties<<<
        public DateTimeOffset? DateProgrammed { get; set; }
        public DateTimeOffset? DateStart { get; set; }
        public DateTimeOffset? DateEnd { get; set; }
        public DateTimeOffset? DateCertification { get; set; }
        public DateTimeOffset? DateExpiresAt { get; set; }
        public string Trainer { get; set; }
        public string InternalExternal { get; set; }
        public string Notes { get; set; }
        public string DurationInHours { get; set; }
        public bool QuickTraining { get; set; }
        public int CertificationKey { get; set; }
        [ForeignKey("CertificationKey")]
        public Certification CatCertification { get; set; }
        public int? Level1Key { get; set; }
        [ForeignKey("Level1Key")]
        public Level1 Level1 { get; set; }
        public int? SupervisorKey { get; set; }
        [ForeignKey("SupervisorKey")]
        public Employee Supervisor { get; set; }
        ///End:Generated:Properties<<<

        [NotMapped]
        public ICollection<TrainingScore> TrainingScores { get; set; }

        [NotMapped]
        public bool IsExpired { get; set; }

        [NotMapped]
        public bool AboutToExpire { get; set; }

        [NotMapped]
        public string CertificationValue { get; set; }
    }
}
