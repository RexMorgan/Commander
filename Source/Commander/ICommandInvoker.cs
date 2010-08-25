using System;
using Commander.Runtime;

namespace Commander
{
    public interface ICommandInvoker
    {
        void ForNew<TEntity>(IDomainCommand<TEntity> command)
            where TEntity : class;

        void ForExisting<TEntity>(Action<EntityRequest> action, IDomainCommand<TEntity> command)
            where TEntity : class;
    }

    public static class InvocationExtensions
    {
        public static void ForNew<TEntity>(this ICommandInvoker invoker, Action<TEntity> action)
            where TEntity : class
        {
            invoker.ForNew(new LambdaDomainCommand<TEntity>(action));
        }
    }
}