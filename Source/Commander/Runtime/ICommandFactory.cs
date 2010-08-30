using System;
using FubuCore.Binding;

namespace Commander.Runtime
{
    public interface ICommandFactory
    {
        ICompiledCommand BuildCommand(ICommandContext context, ServiceArguments arguments, Guid commandId);
    }
}