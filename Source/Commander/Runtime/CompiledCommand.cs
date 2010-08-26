using System;
using Commander.Commands;

namespace Commander.Runtime
{
    public class CompiledCommand
    {
        private readonly ICommand _command;
        private readonly Func<ICommandContext> _context;

        public CompiledCommand(Func<ICommandContext> context, ICommand command)
        {
            _context = context;
            _command = command;
        }


        public ICommandContext Context
        {
            get { return _context(); }
        }

        public ICommand Command
        {
            get { return _command; }
        }
    }
}