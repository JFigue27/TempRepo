using Reusable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessSpecificLogic.EF
{
    [Table("Level1")]
    public class Level1 : BaseEntity
    {
        public Level1()
        {
            ///Start:Generated:Constructor<<<
            ///End:Generated:Constructor<<<
            
        }

        [Key]
        public int Level1Key { get; set; }

        public override int id { get { return Level1Key; } set { Level1Key = value; } }

        ///Start:Generated:Properties<<<
        public string Value { get; set; }
        public List<Certification> Certifications { get; set; }
        public List<Level2> Level2s { get; set; }
        ///End:Generated:Properties<<<
    }
}
