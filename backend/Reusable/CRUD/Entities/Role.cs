using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reusable
{
    [Table("Role")]
    public class Role : BaseEntity
    {
        public Role()
        {
            ///Start:Generated:Constructor<<<
            ///End:Generated:Constructor<<<

        }

        [Key]
        public int RoleKey { get; set; }

        public override int id { get { return RoleKey; } set { RoleKey = value; } }

        ///Start:Generated:Properties<<<
        public string Name { get; set; }
        public int ApplicationKey { get; set; }
        [ForeignKey("ApplicationKey")]
        public Application Application { get; set; }
        ///End:Generated:Properties<<<

        public ICollection<User> Users { get; set; }
    }
}
