using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reusable
{
    [Table("Approval")]
    public class Approval : BaseDocument
    {
        public Approval()
        {
            ///Start:Generated:Constructor<<<
            ///End:Generated:Constructor<<<
        }

        [Key]
        public int ApprovalKey { get; set; }

        public override int id { get { return ApprovalKey; } set { ApprovalKey = value; } }

        ///Start:Generated:Properties<<<
        public DateTimeOffset DateRequested { get; set; }
        public string RequestDescription { get; set; }
        public DateTimeOffset? DateResponse { get; set; }
        public string ResponseDescription { get; set; }
        public string Status { get; set; }
        public string ForeignType { get; set; }
        public int? ForeignKey { get; set; }
        public ICollection<User> Approvers { get; set; } /*Must be users to login the system and approve,
                                                                        this is different from Notify To, where can be external email addresses.*/
        public int CQAHeaderKey { get; set; }
        public string Type { get; set; }
        public string Hyperlink { get; set; }
        public string Title { get; set; }
        ///End:Generated:Properties<<<

        public int? StatusUpdatedByKey { get; set; }
        [ForeignKey("StatusUpdatedByKey")]
        public User StatusUpdatedBy { get; set; }
    }
}
