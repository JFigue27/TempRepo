using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reusable
{
    [Table("Application")]
    public class Application : BaseDocument
    {
        public Application()
        {
            ///Start:Generated:Constructor<<<
            ///End:Generated:Constructor<<<
        }

        [Key]
        public int ApplicationKey { get; set; }

        public override int id { get { return ApplicationKey; } set { ApplicationKey = value; } }

        ///Start:Generated:Properties<<<
        public string Name { get; set; }
        public ICollection<Role> Roles { get; set; }
        ///End:Generated:Properties<<<
    }
}
