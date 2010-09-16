using System;
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
            InvocationResult<TEntity> result;
            if(!_context.Has<InvocationResult<TEntity>>())
            {
                _context.Set(result = new InvocationResult<TEntity>(entity));
            }
            else
            {
                result = _context.Get<InvocationResult<TEntity>>();
            }

            if(exception != null)
            {
                result.AddProblem(new InvocationProblem(_inner, exception));
            }
        }
    }
}