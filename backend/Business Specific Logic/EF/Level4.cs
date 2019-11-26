using Reusable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessSpecificLogic.EF
{
    [Table("Level4")]
    public class Level4 : BaseEntity
    {
        public Level4()
        {
            ///Start:Generated:Constructor<<<
            ///End:Generated:Constructor<<<
            
        }

        [Key]
        public int Level4Key { get; set; }

        public override int id { get { return Level4Key; } set { Level4Key = value; } }

        ///Start:Generated:Properties<<<
        public string Value { get; set; }
        public List<Level3> Level3s { get; set; }
        public List<Level5> Level5s { get; set; }
        ///End:Generated:Properties<<<
    }
}
