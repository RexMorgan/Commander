using System;
using FubuCore;
using FubuCore.Util;

namespace Commander.Runtime
{
    public interface IEntityBuilderRegistry
    {
        IEntityBuilder BuilderFor<TEntity>()
            where TEntity : class;

        IEntityBuilder BuilderFor(Type entityType);
    }

    public class EntityBuilderRegistry : IEntityBuilderRegistry
    {
        private readonly Cache<Type, IEntityBuilder> _builders;

        public EntityBuilderRegistry()
        {
            _builders = new Cache<Type, IEntityBuilder>
            {
                OnMissing = t => typeof(DefaultEntityBuilder<>).MakeGenericType(t).GetDefaultInstance().As<IEntityBuilder>()
            };
        }

        public void AddBuilder<TEntity>(IEntityBuilder builder)
            where TEntity : class
        {
            AddBuilder(typeof(TEntity), builder);
        }

        public void AddBuilder(Type type, IEntityBuilder builder)
        {
            _builders.Fill(type, builder);
        }

        public IEntityBuilder BuilderFor<TEntity>()
            where TEntity : class
        {
            return BuilderFor(typeof (TEntity));
        }

        public IEntityBuilder BuilderFor(Type entityType)
        {
            return _builders[entityType];
        }
    }

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