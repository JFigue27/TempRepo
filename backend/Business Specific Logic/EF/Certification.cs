using Reusable;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessSpecificLogic.EF
{
    [Table("Certification")]
    public class Certification : BaseEntity
    {
        public Certification()
        {
            ///Start:Generated:Constructor<<<
            ///End:Generated:Constructor<<<
            
        }

        [Key]
        public int CertificationKey { get; set; }

        public override int id { get { return CertificationKey; } set { CertificationKey = value; } }

        ///Start:Generated:Properties<<<
        public string Value { get; set; }
        public int LifecycleInMonths { get; set; }
        public bool AppliesToDC3 { get; set; }
        public bool VisibleToCard { get; set; }
        public string ThematicArea { get; set; }
        public int ThematicAreaKey { get; set; }
        public List<Level1> Level1s { get; set; }
        ///End:Generated:Properties<<<
    }
}
