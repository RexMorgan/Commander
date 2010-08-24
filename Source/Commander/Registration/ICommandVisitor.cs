using Commander.Registration.Nodes;

namespace Commander.Registration
{
    public interface ICommandVisitor
    {
        void VisitCommand(CommandChain chain);
    }
}