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
                .Compile(_graph.ChainForNew<TEntity>(), command.ToCommandCall())
                .Execute();
        }

        public void ForExisting<TEntity>(IDomainCommand<TEntity> command) where TEntity : class
        {
            _compiler
                .Compile(_graph.ChainForExisting<TEntity>(), command.ToCommandCall())
                .Execute();
        }
    }
}