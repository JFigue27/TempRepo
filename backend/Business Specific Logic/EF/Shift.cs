using Reusable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessSpecificLogic.EF
{
    [Table("Shift")]
    public class Shift : BaseEntity
    {
        public Shift()
        {
            ///Start:Generated:Constructor<<<
            ///End:Generated:Constructor<<<
            
        }

        [Key]
        public int ShiftKey { get; set; }

        public override int id { get { return ShiftKey; } set { ShiftKey = value; } }

        ///Start:Generated:Properties<<<
        public string Value { get; set; }
        ///End:Generated:Properties<<<
    }
}
