using System;
using Commander.Registration;
using Commander.Registration.Nodes;

namespace Commander.Runtime
{
    public interface ICommandCompiler
    {
        ICompilationContext CompileNew<TEntity>(CommandGraph graph, Action<ICommandContext> configure, CommandCall commandCall)
            where TEntity : class;

        ICompilationContext CompileExisting<TEntity>(CommandGraph graph, Action<ICommandContext> configure, CommandCall commandCall)
            where TEntity : class;
    }
}