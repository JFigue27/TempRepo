namespace Reusable
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Track")]
    public partial class Track : BaseEntity
    {
        [Key]
        public int TrackKey { get; set; }

        public int Entity_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Entity_Kind { get; set; }

        public DateTimeOffset Date_CreatedOn { get; set; }

        public DateTimeOffset? Date_EditedOn { get; set; }

        public DateTimeOffset? Date_RemovedOn { get; set; }

        public DateTimeOffset? Date_LastTimeUsed { get; set; }

        public int? User_CreatedByKey { get; set; }
        [ForeignKey("User_CreatedByKey")]
        public virtual User User_CreatedBy { get; set; }

        public int? User_LastEditedByKey { get; set; }
        [ForeignKey("User_LastEditedByKey")]
        public virtual User User_LastEditedBy { get; set; }

        public int? User_RemovedByKey { get; set; }
        [ForeignKey("User_RemovedByKey")]
        public virtual User User_RemovedBy { get; set; }

        public int? User_AssignedToKey { get; set; }
        [ForeignKey("User_AssignedToKey")]
        public virtual User User_AssignedTo { get; set; }

        public int? User_AssignedByKey { get; set; }
        [ForeignKey("User_AssignedByKey")]
        public virtual User User_AssignedBy { get; set; }

        [NotMapped]
        public override int id { get { return TrackKey; } set { TrackKey = value; } }
    }
}
