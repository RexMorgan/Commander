using System;
using Commander.Commands;

namespace Commander
{
    public class InvocationProblem
    {
        private readonly ICommand _command;
        private readonly Exception _exception;

        public InvocationProblem(ICommand command, Exception exception)
        {
            _command = command;
            _exception = exception;
        }

        public Exception Exception
        {
            get { return _exception; }
        }

        public ICommand Command
        {
            get { return _command; }
        }
    }
}