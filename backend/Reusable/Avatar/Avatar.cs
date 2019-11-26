using System.ComponentModel.DataAnnotations.Schema;

namespace Reusable.Attachments
{
    [NotMapped]
    public class Avatar : Attachment
    {
        public string ImageBase64 { get; set; }
    }
}
