using System;
using System.Diagnostics;
using System.Linq;
using Commander.Bootstrapping;
using Commander.Registration;
using Commander.Registration.Graph;
using Commander.Runtime;
using FubuCore.Binding;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;

namespace Commander.StructureMap
{
    public class StructureMapCommanderContainer : ICommanderContainer, ICompiler
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

        public StructureMapCommanderContainer(IContainer container)
        {
            _container = container;
            _registry = new StructureMapCommanderRegistry();
            Register(typeof(IContainer), new ObjectDef
                                             {
                                                 Type = typeof(IContainer),
                                                 Value = container
                                             });
            Register(typeof(ICommanderContainer), new ObjectDef
                                            {
                                                Type = typeof(ICommanderContainer),
                                                Value = this
                                            });
        }

        private bool _initializeSingletonsToWorkAroundSmBug = true;

        public StructureMapCommanderContainer DoNotInitializeSingletons()
        {
            _initializeSingletonsToWorkAroundSmBug = false;
            return this;
        }

        public ICompilationContext Compile(ICommandContext context, ServiceArguments arguments, Guid behaviorId)
        {
            _container.EjectAllInstancesOf<ICommandContext>();

            var container = _container.GetNestedContainer();
            var command = new StructureMapContainerCommand(container, arguments, behaviorId);
            
            var compilationContext = new StructureMapCompilationContext(container, command);
            context.Set<ICompilationContext>(compilationContext);
            container.Configure(x => x.For<ICommandContext>().Use(context));

            return compilationContext;
        }

        public ICompiler BuildCompiler()
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
}