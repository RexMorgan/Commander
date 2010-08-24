using System;
using System.Collections.Generic;
using Commander.Runtime;

namespace Commander.Registration.Dsl
{
    public class CommandRegistry
    {
        private readonly TypePool _types = new TypePool();
        private readonly TypePool _builderTypes = new TypePool();
        private readonly List<IEntityBuilderRegistrationConvention> _entityBuilderConventions = new List<IEntityBuilderRegistrationConvention>();
        private readonly EntityMatcher _entityMatcher = new EntityMatcher();
        private readonly EntityBuilderMatcher _entityBuilderMatcher = new EntityBuilderMatcher();
        private readonly IList<IConfigurationAction> _policies = new List<IConfigurationAction>();
        private readonly List<IConfigurationAction> _conventions = new List<IConfigurationAction>();
        private readonly List<Action> _explicits = new List<Action>();
        private readonly EntityBuilderRegistry _entityBuilderRegistry = new EntityBuilderRegistry();
        public AppliesToExpression Applies { get { return new AppliesToExpression(_types); } }
        public TypeCandidateExpression Entities { get { return new TypeCandidateExpression(_entityMatcher, _types); } }
        public PoliciesExpression Policies { get { return new PoliciesExpression(_policies); } }
        public EntityBuildersExpression EntityBuilders { get { return new EntityBuildersExpression(_entityBuilderMatcher, _builderTypes, _entityBuilderConventions); } }


        public EntityBuilderRegistry BuilderRegistry { get { return _entityBuilderRegistry; } }

        public CommandRegistry()
        {
            addConvention(graph => _entityMatcher.BuildChains(_types, graph));
            _explicits.Add(() => _builderTypes.ImportAssemblies(_types));
            _explicits.Add(() => _entityBuilderMatcher.RegisterBuilders(_builderTypes, _entityBuilderConventions, _entityBuilderRegistry));
        }

        public CommandRegistry(Action<CommandRegistry> configure)
            : this()
        {
            configure(this);
        }

        public CommandGraph BuildGraph(IEntityBuilderFactory entityBuilderFactory)
        {
            _entityBuilderRegistry.SetFactory(entityBuilderFactory);

            var graph = new CommandGraph();

            _conventions.Configure(graph);
            _policies.Configure(graph);

            _explicits.Each(action => action());

            setupServices(graph);

            return graph;
        }

        private void setupServices(CommandGraph graph)
        {
            graph.Services.SetServiceIfNone(graph);
            graph.Services.SetServiceIfNone<ICommandContext, CommandContext>();
            graph.Services.ReplaceService<IEntityBuilderRegistry>(_entityBuilderRegistry);
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