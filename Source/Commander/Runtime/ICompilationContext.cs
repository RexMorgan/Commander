using System;
using Commander.Commands;
using Commander.Registration.Graph;

namespace Commander.Runtime
{
    public interface ICompilationContext : IDisposable
    {
        ICommand Command { get; }
        ICommandContext Context { get; }
        object Get(Type type);
        T Get<T>(ObjectDef def) where T : class;
        T Get<T>();
    }

    public class NulloCompilationContext : ICompilationContext
    {
        public void Dispose()
        {
        }

        public ICommand Command
        {
            get { throw new NotSupportedException(); }
        }

        public ICommandContext Context
        {
            get { throw new NotSupportedException(); }
        }

        public object Get(Type type)
        {
            throw new NotSupportedException();
        }

        public T Get<T>(ObjectDef def) where T : class
        {
            throw new NotSupportedException();
        }

        public T Get<T>()
        {
            throw new NotSupportedException();
        }
    }
}