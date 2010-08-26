using System;
using Commander.Runtime;

namespace Commander.Commands
{
    public class OneInZeroOutCommandInvoker<TCommand, TInput> : BasicCommand 
        where TInput : class
    {
        private readonly Action<TCommand, TInput> _action;
        private readonly TCommand _command;
        private readonly ICommandContext _context;

        public OneInZeroOutCommandInvoker(ICommandContext context, TCommand command,
                                            Action<TCommand, TInput> action)
        {
            _context = context;
            _command = command;
            _action = action;
        }

        protected override DoNext PerformInvoke()
        {
            var input = _context.Get<TInput>();
            _action(_command, input);
            return DoNext.Continue;
        }
    }
}