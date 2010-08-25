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

        public void ForNew<TEntity>(IDomainCommand<TEntity> command) 
            where TEntity : class
        {
            _compiler
                 .CompileNew<TEntity>(_graph, command.ToCommandCall())
                 .Execute();
        }

        public void ForExisting<TEntity>(Action<EntityRequest> action, IDomainCommand<TEntity> command) where TEntity : class
        {
            _compiler
                .CompileExisting<TEntity>(_graph, action, command.ToCommandCall())
                .Execute();
        }
    }
}