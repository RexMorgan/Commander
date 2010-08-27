using System;
using Commander.Registration;
using Commander.Runtime;

namespace Commander
{
    public class CommandInvoker : ICommandInvoker
    {
        private readonly CommandGraph _graph;
        private readonly ICommandCompiler _compiler;
        public CommandInvoker(CommandGraph graph, ICommandCompiler compiler)
        {
            _graph = graph;
            _compiler = compiler;
        }

        public InvocationResult<TEntity> ForNew<TEntity>(IDomainCommand<TEntity> command)
            where TEntity : class
        {
            var compiledCommand = _compiler
                                    .CompileNew<TEntity>(_graph, command.ToCommandCall());
            compiledCommand
                .Command
                .Execute();

            return compiledCommand
                .Context
                .Get<InvocationResult<TEntity>>();
        }

        public InvocationResult<TEntity> ForExisting<TEntity>(IDomainCommand<TEntity> command) 
            where TEntity : class
        {
            return ForExisting(ctx => { }, command);
        }

        public InvocationResult<TEntity> ForExisting<TEntity>(Action<ICommandContext> configure, IDomainCommand<TEntity> command) 
            where TEntity : class
        {
            var compiledCommand = _compiler
                                    .CompileExisting<TEntity>(_graph, configure, command.ToCommandCall());
            compiledCommand
                .Command
                .Execute();

            return compiledCommand
                .Context
                .Get<InvocationResult<TEntity>>();
        }
    }
}