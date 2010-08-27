using System;
using Commander.Bootstrapping;
using Commander.Commands;
using Commander.Registration;
using Commander.Registration.Graph;
using Commander.Registration.Nodes;
using FubuCore.Binding;

namespace Commander.Runtime
{
    public class CommandCompiler : ICommandCompiler
    {
        private readonly IContainerFacility _facility;
        private readonly IEntityBuilderRegistry _builderRegistry;
        public CommandCompiler(IContainerFacility facility, IEntityBuilderRegistry builderRegistry)
        {
            _facility = facility;
            _builderRegistry = builderRegistry;
        }

        public CompiledCommand CompileNew<TEntity>(CommandGraph graph, CommandCall commandCall)
            where TEntity : class
        {
            return Compile(graph.ChainForNew<TEntity>(), ctx => { }, commandCall);
        }

        public CompiledCommand CompileExisting<TEntity>(CommandGraph graph, Action<ICommandContext> configure, CommandCall commandCall)
            where TEntity : class
        {
            var chain = graph.ChainForExisting<TEntity>();
            return Compile(chain, configure, commandCall);
        }

        // Keep this public for testing
        public CompiledCommand Compile(CommandChain chain, Action<ICommandContext> configure, CommandCall commandCall)
        {
            var context = new CommandContext(_builderRegistry);
            configure(context);

            _facility.Register(typeof(ICommandContext), new ObjectDef(typeof(CommandContext))
                                                                {
                                                                    Value = context
                                                                });

            chain
                .Placeholder()
                .ReplaceWith(commandCall);

            _facility.Register(typeof (ICommand), chain.ToObjectDef());

            return _facility
                .BuildFactory()
                .BuildCommand(new ServiceArguments(), chain.UniqueId);
        }
    }
}