using System;
using Commander.Bootstrapping;
using Commander.Commander;
using Commander.Registration;
using Commander.Registration.Graph;
using Commander.Registration.Nodes;
using FubuCore.Binding;

namespace Commander.Runtime
{
    public class CommandCompiler : ICommandCompiler
    {
        private readonly IContainerFacility _facility;

        public CommandCompiler(IContainerFacility facility)
        {
            _facility = facility;
        }

        public ICommand CompileNew<TEntity>(CommandGraph graph, CommandCall commandCall)
            where TEntity : class
        {
            return Compile(graph.ChainForNew<TEntity>(), commandCall);
        }

        public ICommand CompileExisting<TEntity>(CommandGraph graph, Action<EntityRequest> action, CommandCall commandCall)
            where TEntity : class
        {
            var request = new EntityRequest();
            action(request);
            _facility.Register(typeof(EntityRequest), new ObjectDef(typeof(EntityRequest))
                                                          {
                                                              Value = request
                                                          });

            var chain = graph.ChainForExisting<TEntity>();
            chain.Prepend(new EntityRequestNode());
            return Compile(chain, commandCall);
        }

        // Keep this public for testing
        public ICommand Compile(CommandChain chain, CommandCall commandCall)
        {
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