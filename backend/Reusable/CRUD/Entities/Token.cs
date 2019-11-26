using Reusable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessSpecificLogic.EF
{
    [Table("Token")]
    public class Token : BaseEntity
    {
        public Token()
        {
            ///Start:Generated:Constructor<<<
			CreatedAt = DateTimeOffset.Now;
			///End:Generated:Constructor<<<

            
        }

        [Key]
        public int TokenKey { get; set; }

        public override int id { get { return TokenKey; } set { TokenKey = value; } }

        ///Start:Generated:Properties<<<
        public string Value { get; set; }
        public string Subject { get; set; }
        public int SubjectKey { get; set; }
        public DateTimeOffset? DeadDate { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        ///End:Generated:Properties<<<
    }
}
