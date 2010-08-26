using System;
using System.Collections.Generic;
using Commander.Runtime;

namespace Commander.Commands
{
    public class InvocationTracer<TEntity> : ICommand
        where TEntity : class
    {
        private readonly ICommandContext _context;
        private readonly ICommand _inner;

        public InvocationTracer(ICommandContext context, ICommand inner)
        {
            _context = context;
            _inner = inner;
        }

        public void Execute()
        {
            Exception exception = null;

            try
            {
                _inner.Execute();
            }
            catch (Exception exc)
            {
                exception = exc;
            }

            var entity = _context.Get<TEntity>();
            var problems = new List<InvocationProblem>();
            if(exception != null)
            {
                problems.Add(new InvocationProblem(_inner, exception));
            }

            _context.Set(new InvocationResult<TEntity>(entity, problems));
        }
    }
}