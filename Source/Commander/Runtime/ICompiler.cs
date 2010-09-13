using System;
using FubuCore.Binding;

namespace Commander.Runtime
{
    public interface ICompiler
    {
        ICompilationContext Compile(ICommandContext context, ServiceArguments arguments, Guid commandId);
    }
}