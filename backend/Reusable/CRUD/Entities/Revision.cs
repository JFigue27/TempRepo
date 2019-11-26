using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reusable
{
    [Table("Revision")]
    public class Revision : BaseDocument
    {
        public Revision()
        {
            ///Start:Generated:Constructor<<<
			///End:Generated:Constructor<<<
        }

        [Key]
        public int RevisionKey { get; set; }

        public override int id { get { return RevisionKey; } set { RevisionKey = value; } }

        ///Start:Generated:Properties<<<
        public string ForeignType { get; set; }
        public int ForeignKey { get; set; }
        public string Value { get; set; }
        ///End:Generated:Properties<<<
    }
}
