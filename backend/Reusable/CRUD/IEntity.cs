using System;

namespace Reusable
{
    public interface IEntity : ICloneable
    {
        int id { get; set; }

        string AAA_EntityName { get; }
    }
}