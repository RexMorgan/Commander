using System;

namespace Commander.Runtime
{
    public class DefaultEntityBuilder<TEntity> : IEntityBuilder
        where TEntity : class
    {
        public Type EntityType
        {
            get { return typeof(TEntity); }
        }

        public object Build()
        {
            return typeof(TEntity).GetDefaultInstance();
        }
    }
}