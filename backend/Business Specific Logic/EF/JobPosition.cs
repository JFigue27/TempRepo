using Reusable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessSpecificLogic.EF
{
    [Table("JobPosition")]
    public class JobPosition : BaseEntity
    {
        public JobPosition()
        {
            ///Start:Generated:Constructor<<<
            ///End:Generated:Constructor<<<
            
        }

        [Key]
        public int JobPositionKey { get; set; }

        public override int id { get { return JobPositionKey; } set { JobPositionKey = value; } }

        ///Start:Generated:Properties<<<
        public string Value { get; set; }
        public string Description { get; set; }
        ///End:Generated:Properties<<<

        public List<Skill> Skills { get; set; }
    }
}
