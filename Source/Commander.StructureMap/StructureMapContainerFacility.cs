using System;
using System.Diagnostics;
using System.Linq;
using Commander.Bootstrapping;
using Commander.Commander;
using Commander.Registration;
using Commander.Registration.Graph;
using Commander.Runtime;
using FubuCore.Binding;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;

namespace Commander.StructureMap
{
    public class StructureMapContainerFacility : IContainerFacility, ICommandFactory
    {
        private readonly IContainer _container;
        private readonly Registry _registry;

        public Func<IContainer, ServiceArguments, Guid, ICommand> Builder =
            (container, args, behaviorId) => new NestedStructureMapContainerCommand(container, args, behaviorId);

        public IContainer Container
        {
            get
            {
                return _container;
            }
        }

        public StructureMapContainerFacility(IContainer container)
        {
            _container = container;
            _registry = new StructureMapCommanderRegistry();
        }

        private bool _initializeSingletonsToWorkAroundSmBug = true;

        public StructureMapContainerFacility DoNotInitializeSingletons()
        {
            _initializeSingletonsToWorkAroundSmBug = false;
            return this;
        }

        public ICommand BuildCommand(ServiceArguments arguments, Guid behaviorId)
        {
            return Builder(_container, arguments, behaviorId);
        }

        public ICommandFactory BuildFactory()
        {
            _registry.For<ICommandFactory>().Use(() => new PartialCommandFactory(_container.GetNestedContainer()));
            _container.Configure(x =>
            {
                x.AddRegistry(_registry);
            });

            if (_initializeSingletonsToWorkAroundSmBug)
            {
                initialize_Singletons_to_work_around_StructureMap_GitHub_Issue_3();
            }

            return this;
        }

        private void initialize_Singletons_to_work_around_StructureMap_GitHub_Issue_3()
        {
            var allSingletons = _container.Model.PluginTypes.Where(x => x.Lifecycle == InstanceScope.Singleton.ToString());
            Debug.WriteLine("Found singletons: " + allSingletons.Count());
            foreach (var pluginType in allSingletons)
            {
                var instance = _container.GetInstance(pluginType.PluginType);
                Debug.WriteLine("Initialized singleton in primary container: " + instance);
            }
        }

        public void Register(Type serviceType, ObjectDef def)
        {
            if (def.Value == null)
            {
                _registry.For(serviceType).Add(new ObjectDefInstance(def));
            }
            else
            {
                _registry.For(serviceType).Add(new ObjectInstance(def.Value));
            }

            if (ServiceRegistry.ShouldBeSingleton(serviceType))
            {
                _registry.For(serviceType).Singleton();
            }
        }
    }

    public class PartialCommandFactory : ICommandFactory
    {
        private readonly IContainer _container;

        public PartialCommandFactory(IContainer container)
        {
            _container = container;
        }

        public ICommand BuildCommand(ServiceArguments arguments, Guid commandId)
        {
            return _container.GetInstance<ICommand>(arguments.ToExplicitArgs(), commandId.ToString());
        }
    }
}