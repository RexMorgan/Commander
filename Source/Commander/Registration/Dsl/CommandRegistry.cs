using System;
using System.Collections.Generic;
using System.Linq;
using Commander.Diagnostics;
using Commander.Runtime;

namespace Commander.Registration.Dsl
{
    public class CommandRegistry
    {
        private IConfigurationObserver _observer;
        private readonly TypePool _types = new TypePool();
        private readonly TypePool _builderTypes = new TypePool();
        private readonly List<IEntityBuilderRegistrationConvention> _entityBuilderConventions = new List<IEntityBuilderRegistrationConvention>();
        private readonly EntityMatcher _entityMatcher = new EntityMatcher();
        private readonly EntityBuilderMatcher _entityBuilderMatcher = new EntityBuilderMatcher();
        private readonly IList<IConfigurationAction> _policies = new List<IConfigurationAction>();
        private readonly List<IConfigurationAction> _conventions = new List<IConfigurationAction>();
        private readonly List<Action<CommandGraph>> _explicits = new List<Action<CommandGraph>>();
        private readonly EntityBuilderRegistry _entityBuilderRegistry = new EntityBuilderRegistry();
        private readonly List<CommandRegistry> _imports = new List<CommandRegistry>();
        public AppliesToExpression Applies { get { return new AppliesToExpression(_types); } }
        public TypeCandidateExpression Entities { get { return new TypeCandidateExpression(_entityMatcher, _types); } }
        public PoliciesExpression Policies { get { return new PoliciesExpression(_policies); } }
        public EntityBuildersExpression EntityBuilders { get { return new EntityBuildersExpression(_entityBuilderMatcher, _builderTypes, _entityBuilderConventions); } }
        private readonly List<IConfigurationAction> _systemPolicies = new List<IConfigurationAction>();


        public EntityBuilderRegistry BuilderRegistry { get { return _entityBuilderRegistry; } }

        public CommandRegistry()
        {
            _observer = new NulloConfigurationObserver();

            addConvention(graph => _entityMatcher.BuildChains(_types, graph));
            _explicits.Add(graph => _builderTypes.ImportAssemblies(_types));
            _explicits.Add(graph => _entityBuilderMatcher.RegisterBuilders(_builderTypes, _entityBuilderConventions, _entityBuilderRegistry));
        }

        public CommandRegistry(Action<CommandRegistry> configure)
            : this()
        {
            configure(this);
        }

        public void UsingObserver(IConfigurationObserver observer)
        {
            _observer = observer;
        }

        public CommandGraph BuildGraph(IEntityBuilderFactory entityBuilderFactory)
        {
            ApplySystemPolicy<InvocationTracerPrepender>();

            _entityBuilderRegistry.SetFactory(entityBuilderFactory);

            var graph = new CommandGraph(_observer);

            _conventions.Configure(graph);
            _policies.Configure(graph);

            _explicits.Each(action => action(graph));

            _systemPolicies.Configure(graph);

            setupServices(graph);

            return graph;
        }

        public void Services(Action<IServiceRegistry> configure)
        {
            var action = new LambdaConfigurationAction(g => configure(g.Services));
            _conventions.Add(action);
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

        public void Modify<TRegistryModification>() 
            where TRegistryModification : IRegistryModification, new()
        {
            new TRegistryModification().Modify(this);
        }

        public void Import<TRegistry>()
            where TRegistry : CommandRegistry, new()
        {
            if (_imports.Any(x => x is TRegistry))
            {
                return;
            }

            Import(new TRegistry());
        }

        public void Import(CommandRegistry registry)
        {
            _imports.Add(registry);
        }

        public void ApplySystemPolicy<TConfigurationAction>()
            where TConfigurationAction : IConfigurationAction, new()
        {
            _systemPolicies.Add(new TConfigurationAction());
        }
    }
}