using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reusable.Workflows
{
    [Table("Workflow")]
    public partial class Workflow : BaseEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Workflow()
        {
        }

        [Key]
        public int WorkflowKey { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        public override int id { get { return WorkflowKey; } set { WorkflowKey = value; } }

        public List<Step> Steps { get; set; }
    }
}