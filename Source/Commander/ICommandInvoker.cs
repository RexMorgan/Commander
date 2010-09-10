using System;
using Commander.Runtime;

namespace Commander
{
    public interface ICommandInvoker
    {
        InvocationResult<TEntity> ForNew<TEntity>(IDomainCommand<TEntity> command)
            where TEntity : class;

        InvocationResult<TEntity> ForNew<TEntity, TCommand>()
            where TEntity : class
            where TCommand : IDomainCommand<TEntity>;

        InvocationResult<TEntity> ForNew<TEntity, TCommand>(Action<ICommandContext> configure)
            where TEntity : class
            where TCommand : IDomainCommand<TEntity>;

        InvocationResult<TEntity> ForNew<TEntity>(Action<ICommandContext> configure)
            where TEntity : class;

        InvocationResult<TEntity> ForNew<TEntity>(Action<ICommandContext> configure, IDomainCommand<TEntity> command)
            where TEntity : class;

        InvocationResult<TEntity> ForExisting<TEntity>(IDomainCommand<TEntity> command)
            where TEntity : class;

        InvocationResult<TEntity> ForExisting<TEntity>(Action<ICommandContext> configure, IDomainCommand<TEntity> command)
            where TEntity : class;

        InvocationResult<TEntity> ForExisting<TEntity, TCommand>()
            where TEntity : class
            where TCommand : IDomainCommand<TEntity>;

        InvocationResult<TEntity> ForExisting<TEntity>(Action<ICommandContext> configure)
            where TEntity : class;

        InvocationResult<TEntity> ForExisting<TEntity, TCommand>(Action<ICommandContext> configure)
            where TEntity : class
            where TCommand : IDomainCommand<TEntity>;
    }
}