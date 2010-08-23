using Commander.Commander;
using Commander.Registration.Nodes;

namespace Commander.Runtime
{
    public interface ICommandCompiler
    {
        ICommand Compile(CommandChain chain, CommandCall commandCall);
    }
}