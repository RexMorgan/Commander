using System;
using System.Collections.Generic;
using Commander.Runtime;

namespace Commander.Registration.Dsl
{
    public class CommandRegistry
    {
        private readonly TypePool _types = new TypePool();
        private readonly EntityMatcher _entityMatcher = new EntityMatcher();
        private readonly IList<IConfigurationAction> _policies = new List<IConfigurationAction>();
        private readonly List<IConfigurationAction> _conventions = new List<IConfigurationAction>();
        public AppliesToExpression Applies { get { return new AppliesToExpression(_types); } }
        public EntityCandidateExpression Entities { get { return new EntityCandidateExpression(_entityMatcher, _types); } }
        public PoliciesExpression Policies { get { return new PoliciesExpression(_policies); } }

        public CommandRegistry()
        {
            addConvention(graph => _entityMatcher.BuildBehaviors(_types, graph));
        }

        public CommandRegistry(Action<CommandRegistry> configure)
            : this()
        {
            configure(this);
        }

        public CommandGraph BuildGraph()
        {
            var graph = new CommandGraph();

            _conventions.Configure(graph);
            _policies.Configure(graph);

            setupServices(graph);

            return graph;
        }

        private void setupServices(CommandGraph graph)
        {
            graph.Services.SetServiceIfNone(graph);
            graph.Services.SetServiceIfNone<ICommandContext, CommandContext>();
            graph.Services.SetServiceIfNone<ICommandCompiler, CommandCompiler>();
            graph.Services.SetServiceIfNone<IEntityBuilderRegistry, EntityBuilderRegistry>();
        }

        private void addConvention(Action<CommandGraph> convention)
        {
            _conventions.Add(new LambdaConfigurationAction(convention));
        }

        public void ApplyConvention<TConvention>()
           where TConvention : IConfigurationAction, new()
        {
            ApplyConvention(new TConvention());
        }

        public void ApplyConvention<TConvention>(TConvention convention)
            where TConvention : IConfigurationAction
        {
            _conventions.Add(convention);
        }
    }
}