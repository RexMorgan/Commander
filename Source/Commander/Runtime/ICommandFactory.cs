using System;
using FubuCore.Binding;

namespace Commander.Runtime
{
    public interface ICommandFactory
    {
        CompiledCommand BuildCommand(ServiceArguments arguments, Guid commandId);
    }
}