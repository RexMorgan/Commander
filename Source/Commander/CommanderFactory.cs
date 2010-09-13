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

        public static void Initialize<TRegistry>(ICommanderContainer container)
            where TRegistry : CommandRegistry, new()
        {
            Initialize(container, new TRegistry());
        }


        public static void Initialize(ICommanderContainer container, CommandRegistry registry)
        {
            lock(typeof(CommanderFactory))
            {
                _graph = registry.BuildGraph();
                _graph
                    .Services
                    .ReplaceService<ICommanderContainer>(container);
                
                _graph.EachService(container.Register);
                registry.BuilderRegistry.EachBuilder(container.Register);

                _invoker = new CommandInvoker(_graph, new CommandCompiler(container, registry.BuilderRegistry));
            }
        }

        public static void Initialize(ICommanderContainer container, Action<CommandRegistry> configure)
        {
            Initialize(container, new CommandRegistry(configure));
        }
    }
}