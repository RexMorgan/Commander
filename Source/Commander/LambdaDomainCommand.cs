using System;

namespace Commander
{
    public class LambdaDomainCommand<TEntity> : IDomainCommand<TEntity>
        where TEntity : class
    {
        private readonly Action<TEntity> _action;

        public LambdaDomainCommand(Action<TEntity> action)
        {
            _action = action;
        }

        public void Execute(TEntity entity)
        {
            _action(entity);
        }
    }
}