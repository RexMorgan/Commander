namespace Commander.Diagnostics
{
    public interface ICommandDetails
    {
        void AcceptVisitor(ICommandDetailsVisitor visitor);
    }
}