using System;

namespace Commander.Runtime
{
    public interface IEntityBuilder
    {
        Type EntityType { get; }
        object Build();
    }
}