using System;
using System.Collections.Generic;
using Commander.Runtime;
using FubuCore.Util;

namespace Commander.Registration.Dsl
{
    public class EntityBuilderMatcher : ITypeMatcher
    {
        private readonly CompositeFilter<Type> _typeFilters = new CompositeFilter<Type>();
        public CompositeFilter<Type> TypeFilters { get { return _typeFilters; } }

        public void RegisterBuilders(TypePool pool, IEnumerable<IEntityBuilderRegistrationConvention> conventions, EntityBuilderRegistry registry)
        {
            pool
                .TypesMatching(TypeFilters.Matches)
                .Each(t => conventions.Each(convention => convention.Process(t, registry)));
        }
    }
}