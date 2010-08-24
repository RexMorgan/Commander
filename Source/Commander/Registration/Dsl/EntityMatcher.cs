using System;
using System.Collections.Generic;
using Commander.Registration.Nodes;
using FubuCore.Util;

namespace Commander.Registration.Dsl
{
    public class EntityMatcher
    {
        private readonly CompositeFilter<Type> _typeFilters = new CompositeFilter<Type>();
        public CompositeFilter<Type> TypeFilters { get { return _typeFilters; } }

        public void BuildBehaviors(TypePool pool, CommandGraph graph)
        {
            pool
                .TypesMatching(TypeFilters.Matches)
                .Each(t =>
                          {
                              graph.AddChainForNew(new CommandChain(t));
                              graph.AddChainForExisting(new CommandChain(t));
                          });
        }
    }
}