using Reusable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessSpecificLogic.EF
{
    [Table("FormatoDC3")]
    public class FormatoDC3 : BaseDocument
    {
        public FormatoDC3()
        {
            ///Start:Generated:Constructor<<<
            ///End:Generated:Constructor<<<

            
        }

        [Key]
        public int FormatoDC3Key { get; set; }

        public override int id { get { return FormatoDC3Key; } set { FormatoDC3Key = value; } }

        ///Start:Generated:Properties<<<
        public string SpecificOccupation { get; set; }
        public string CourseThematicArea { get; set; }
        public int TrainingScoreKey { get; set; }
        [ForeignKey("TrainingScoreKey")]
        public TrainingScore TrainingScore { get; set; }
        public int? EmployeeKey { get; set; }
        [ForeignKey("EmployeeKey")]
        public Employee Employee { get; set; }
        public int? CertificationKey { get; set; }
        [ForeignKey("CertificationKey")]
        public Certification Certification { get; set; }
        ///End:Generated:Properties<<<
    }
}
