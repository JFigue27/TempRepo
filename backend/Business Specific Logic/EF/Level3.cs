using Reusable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessSpecificLogic.EF
{
    [Table("Level3")]
    public class Level3 : BaseEntity
    {
        public Level3()
        {
            ///Start:Generated:Constructor<<<
            ///End:Generated:Constructor<<<
            
        }

        [Key]
        public int Level3Key { get; set; }

        public override int id { get { return Level3Key; } set { Level3Key = value; } }

        ///Start:Generated:Properties<<<
        public string Value { get; set; }
        public List<Level2> Level2s { get; set; }
        public List<Level4> Level4s { get; set; }
        ///End:Generated:Properties<<<
    }
}
