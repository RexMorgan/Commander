using System;
using Commander.Bootstrapping;
using Commander.Registration;
using Commander.Registration.Dsl;
using Commander.Runtime;

namespace Commander
{
    public static class CommanderFactory
    {
        private static CommandGraph _graph;
        private static ICommandInvoker _invoker;

        public static CommandGraph Graph { get { return _graph; } }
        public static ICommandInvoker Invoker { get { return _invoker; } }

        public static void Initialize<TRegistry>(IContainerFacility facility)
            where TRegistry : CommandRegistry, new()
        {
            Initialize(facility, new TRegistry());
        }


        public static void Initialize(IContainerFacility facility, CommandRegistry registry)
        {
            lock(typeof(CommanderFactory))
            {
                _graph = registry.BuildGraph();
                _graph
                    .Services
                    .ReplaceService<IContainerFacility>(facility);
                _graph.EachService(facility.Register);

                _invoker = new CommandInvoker(_graph, new CommandCompiler(facility));
            }
        }

        public static void Initialize(IContainerFacility facility, Action<CommandRegistry> configure)
        {
            var registry = new CommandRegistry(configure);
            Initialize(facility, registry);
        }
    }
}