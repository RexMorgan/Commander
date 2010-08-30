using System;
using Commander.Commands;

namespace Commander.Runtime
{
    public interface ICompiledCommand : IDisposable
    {
        ICommand Command { get; }
        ICommandContext Context { get; }
    }
}