using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reusable.Workflows
{
    [Table("WorkflowStep")]
    public partial class Step : BaseEntity, IRecursiveEntity<Step>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Step()
        {
        }

        [Key]
        public int StepKey { get; set; }
        public override int id { get { return StepKey; } set { StepKey = value; } }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }


        public int? WorkflowKey { get; set; }
        public string ProcessType { get; set; }

        [ForeignKey("ParentKey")]
        public Step ParentStep { get; set; }
        public int? ParentKey { get; set; }

        public List<Step> Steps { get; set; }

        [NotMapped]
        public List<Step> nodes { get; set; }
    }
}
