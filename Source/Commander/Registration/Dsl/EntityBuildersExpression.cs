using System;
using System.Collections.Generic;
using Commander.Runtime;
using FubuCore;

namespace Commander.Registration.Dsl
{
    public class EntityBuildersExpression
    {
        private readonly ITypeMatcher _matcher;
        private readonly TypePool _types;
        private readonly IList<IEntityBuilderRegistrationConvention> _conventions;
        public EntityBuildersExpression(ITypeMatcher matcher, TypePool types, IList<IEntityBuilderRegistrationConvention> conventions)
        {
            _matcher = matcher;
            _types = types;
            _conventions = conventions;

            _matcher.TypeFilters.Includes += (type => !type.IsOpenGeneric() && type.IsConcreteTypeOf<IEntityBuilder>());
            _matcher.TypeFilters.Includes += (type => type.ImplementsInterfaceTemplate(typeof(IEntityBuilder<>)));
        }

        public EntityBuildersExpression Include<T>()
            where T : IEntityBuilder
        {
            _types.AddType(typeof(T));
            _matcher.TypeFilters.Includes += type => type == typeof(T);
            return this;
        }

        public EntityBuildersExpression Exclude<T>()
            where T : IEntityBuilder
        {
            _matcher.TypeFilters.Excludes += (type => type.Equals(typeof(T)));
            return this;
        }

        public EntityBuildersExpression ApplyConvention<TFinder>()
            where TFinder : IEntityBuilderRegistrationConvention, new()
        {
            ApplyConvention(new TFinder());
            return this;
        }

        public EntityBuildersExpression ApplyConvention(IEntityBuilderRegistrationConvention builderRegistrationConvention)
        {
            _conventions.Add(builderRegistrationConvention);
            return this;
        }

        public EntityBuildersExpression RegisterAllAvailable()
        {
            ApplyConvention<RegisterAllAvailableEntityBuilders>();
            return this;
        }
    }

    public class RegisterAllAvailableEntityBuilders : IEntityBuilderRegistrationConvention
    {
        public void Process(Type type, EntityBuilderRegistry registry)
        {
            var interfaceType = type.FindFirstInterfaceThatCloses(typeof (IEntityBuilder<>));
            if (interfaceType != null)
            {
                registry
                    .AddBuilder(interfaceType.GetGenericArguments()[0], type); // shit...object defs for builders too?
            }
        }
    }

    public interface IEntityBuilderRegistrationConvention
    {
        void Process(Type type, EntityBuilderRegistry registry);
    }
}