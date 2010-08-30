using System;
using Commander.Registration;
using Commander.Registration.Nodes;

namespace Commander.Runtime
{
    public interface ICommandCompiler
    {
        ICompiledCommand CompileNew<TEntity>(CommandGraph graph, Action<ICommandContext> configure, CommandCall commandCall)
            where TEntity : class;

        ICompiledCommand CompileExisting<TEntity>(CommandGraph graph, Action<ICommandContext> configure, CommandCall commandCall)
            where TEntity : class;
    }
}