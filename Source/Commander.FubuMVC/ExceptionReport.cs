namespace Commander.Diagnostics
{
    public class ExceptionReport : ICommandDetails
    {
        public string Text { get; set; }

        public void AcceptVisitor(ICommandDetailsVisitor visitor)
        {
            visitor.Exception(this);
        }
    }
}