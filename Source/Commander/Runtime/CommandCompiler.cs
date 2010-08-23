using Commander.Bootstrapping;
using Commander.Commander;
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