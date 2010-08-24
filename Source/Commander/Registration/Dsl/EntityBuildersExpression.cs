using System;
using System.Collections.Generic;
using Commander.Runtime;

namespace Commander.Registration.Dsl
{
    public class EntityBuildersExpression : TypeCandidateExpression
    {
        private readonly IList<IEntityBuilderRegistrationConvention> _conventions;
        public EntityBuildersExpression(ITypeMatcher matcher, TypePool types, IList<IEntityBuilderRegistrationConvention> conventions)
            : base(matcher, types)
        {
            _conventions = conventions;
        }

        public void AddFinder<TFinder>()
            where TFinder : IEntityBuilderRegistrationConvention, new()
        {
            AddFinder(new TFinder());
        }

        public void AddFinder(IEntityBuilderRegistrationConvention builderRegistrationConvention)
        {
            _conventions.Add(builderRegistrationConvention);
        }

        public void RegisterAllAvailable()
        {
            AddFinder<RegisterAllAvailableEntityBuilders>();
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