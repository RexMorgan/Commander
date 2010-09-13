using System;
using System.Linq;
using Commander.Registration.Graph;
using FubuCore.Util;

namespace Commander.Runtime
{
    public class EntityBuilderRegistry : IEntityBuilderRegistry
    {
        private readonly Cache<Type, ObjectDef> _builders;

        public EntityBuilderRegistry()
        {
            _builders = new Cache<Type, ObjectDef>
                            {
                                OnMissing = t => new ObjectDef(typeof(DefaultEntityBuilder<>).MakeGenericType(t))
                            };
        }

        public void EachBuilder(Action<Type, ObjectDef> action)
        {
            _builders
                .GetAll()
                .ToList()
                .ForEach(d => action(d.Type, d));
        }

        public void AddBuilder<TEntity>(IEntityBuilder builder)
            where TEntity : class
        {
            AddBuilder(typeof(TEntity), builder);
        }

        public void AddBuilder(Type type, IEntityBuilder builder)
        {
            AddBuilder(type, new ObjectDef(builder.GetType())
                                 {
                                     Value = builder
                                 });
        }

        public void AddBuilder(Type entityType, Type builderType)
        {
            AddBuilder(entityType, new ObjectDef(builderType));
        }

        public void AddBuilder(Type entityType, ObjectDef builderDef)
        {
            _builders.Fill(entityType, builderDef);
        }

        public ObjectDef BuilderFor<TEntity>()
            where TEntity : class
        {
            return BuilderFor(typeof (TEntity));
        }

        public ObjectDef BuilderFor(Type entityType)
        {
            return _builders[entityType];
        }
    }
}