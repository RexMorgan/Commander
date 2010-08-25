namespace Commander.Diagnostics
{
    public interface ICommandDetailsVisitor
    {
        void Exception(ExceptionReport report);
        void SetValue(SetValueReport report);
    }
}