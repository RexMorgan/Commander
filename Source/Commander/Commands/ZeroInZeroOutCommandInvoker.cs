using System;

namespace Commander.Commander
{
    public class ZeroInZeroOutCommandInvoker<TCommand> : BasicCommand
    {
        private readonly Action<TCommand> _action;
        private readonly TCommand _command;
        public ZeroInZeroOutCommandInvoker(TCommand command, Action<TCommand> action)
        {
            _command = command;
            _action = action;
        }

        protected override DoNext PerformInvoke()
        {
            _action(_command);
            return DoNext.Continue;
        }
    }
}