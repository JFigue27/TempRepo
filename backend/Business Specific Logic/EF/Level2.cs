using Reusable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessSpecificLogic.EF
{
    [Table("Level2")]
    public class Level2 : BaseEntity
    {
        public Level2()
        {
            ///Start:Generated:Constructor<<<
            ///End:Generated:Constructor<<<
            
        }

        [Key]
        public int Level2Key { get; set; }

        public override int id { get { return Level2Key; } set { Level2Key = value; } }

        ///Start:Generated:Properties<<<
        public string Value { get; set; }
        public List<Level1> Level1s { get; set; }
        public List<Level3> Level3s { get; set; }
        ///End:Generated:Properties<<<
    }
}
