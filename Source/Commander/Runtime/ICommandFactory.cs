using System;
using Commander.Commander;
using FubuCore.Binding;

namespace Commander.Runtime
{
    public interface ICommandFactory
    {
        ICommand BuildCommand(ServiceArguments arguments, Guid commandId);
    }
}