using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reusable.Attachments
{
    public interface IAvatar
    {
        string AvatarFolder { get; set; }

        [NotMapped]
        List<Avatar> AvatarList { get; set; }
    }
}
