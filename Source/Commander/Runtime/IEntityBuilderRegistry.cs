using System;
using Commander.Registration.Graph;

namespace Commander.Runtime
{
    public interface IEntityBuilderRegistry
    {
        ObjectDef BuilderFor<TEntity>()
            where TEntity : class;

        ObjectDef BuilderFor(Type entityType);
    }
}