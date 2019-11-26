using Reusable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessSpecificLogic.EF
{
    [Table("Level5")]
    public class Level5 : BaseEntity
    {
        public Level5()
        {
            ///Start:Generated:Constructor<<<
            ///End:Generated:Constructor<<<
            
        }

        [Key]
        public int Level5Key { get; set; }

        public override int id { get { return Level5Key; } set { Level5Key = value; } }

        ///Start:Generated:Properties<<<
        public string Value { get; set; }
        public List<Level4> Level4s { get; set; }
        ///End:Generated:Properties<<<
    }
}
