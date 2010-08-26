using System;
using Commander.Commands;
using Commander.Registration;
using Commander.Registration.Nodes;

namespace Commander.Runtime
{
    public interface ICommandCompiler
    {
        CompiledCommand CompileNew<TEntity>(CommandGraph graph, CommandCall commandCall)
            where TEntity : class;

        CompiledCommand CompileExisting<TEntity>(CommandGraph graph, Action<EntityRequest> action, CommandCall commandCall)
            where TEntity : class;
    }
}