using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reusable
{
    public interface IRecursiveEntity<T> : IEntity where T : IEntity
    {
        [NotMapped]
        List<T> nodes { get; set; }
    }
}
