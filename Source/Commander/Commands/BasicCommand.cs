namespace Commander.Commands
{
    public class BasicCommand : ICommand
    {
        public ICommand InsideCommand { get; set; }

        public void Execute()
        {
            if (PerformInvoke() == DoNext.Continue && InsideCommand != null)
            {
                InsideCommand.Execute();
            }

            AfterInsideBehavior();
        }

        protected virtual DoNext PerformInvoke()
        {
            return DoNext.Continue;
        }

        protected virtual void AfterInsideBehavior()
        {   
        }
    }
}