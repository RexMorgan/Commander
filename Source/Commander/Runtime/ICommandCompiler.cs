using System;
using Commander.Commander;
using Commander.Registration;
using Commander.Registration.Nodes;

namespace Commander.Runtime
{
    public interface ICommandCompiler
    {
        ICommand CompileNew<TEntity>(CommandGraph graph, CommandCall commandCall)
            where TEntity : class;

        ICommand CompileExisting<TEntity>(CommandGraph graph, Action<EntityRequest> action, CommandCall commandCall)
            where TEntity : class;
    }
}