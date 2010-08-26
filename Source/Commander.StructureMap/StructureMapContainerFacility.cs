using System;
using System.Diagnostics;
using System.Linq;
using Commander.Bootstrapping;
using Commander.Commands;
using Commander.Registration;
using Commander.Registration.Graph;
using Commander.Runtime;
using FubuCore.Binding;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;

namespace Commander.StructureMap
{
    public class StructureMapContainerFacility : IContainerFacility, ICommandFactory, IEntityBuilderFactory
    {
        private readonly IContainer _container;
        private readonly Registry _registry;

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
            Register(typeof(IContainer), new ObjectDef
                                             {
                                                 Type = typeof(IContainer),
                                                 Value = container
                                             });
            Register(typeof(IContainerFacility), new ObjectDef
                                            {
                                                Type = typeof(IContainerFacility),
                                                Value = this
                                            });
        }

        private bool _initializeSingletonsToWorkAroundSmBug = true;

        public StructureMapContainerFacility DoNotInitializeSingletons()
        {
            _initializeSingletonsToWorkAroundSmBug = false;
            return this;
        }

        public CompiledCommand BuildCommand(ServiceArguments arguments, Guid behaviorId)
        {
            var container = _container.GetNestedContainer();
            var command = new StructureMapContainerCommand(container, arguments, behaviorId);
            return new CompiledCommand(container.GetInstance<ICommandContext>, command);
        }

        public ICommandFactory BuildFactory()
        {
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

        public IEntityBuilderFactory BuildEntityBuilderFactory()
        {
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

        public IEntityBuilder Build(ObjectDef builderDef)
        {
            using(var container = _container.GetNestedContainer())
            {
                return (IEntityBuilder)container.GetInstance(builderDef.Type);
            }
        }
    }
}