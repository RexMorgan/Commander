namespace Commander.Diagnostics
{
    public class CommandStep
    {
        public CommandReport Command { get; set; }
        public ICommandDetails Details { get; set; }
    }
}