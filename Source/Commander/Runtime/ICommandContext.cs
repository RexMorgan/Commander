using System;

namespace Commander.Runtime
{
    public interface ICommandContext
    {
        T Get<T>() where T : class;
        object Get(Type type);
        void Set<T>(T target) where T : class;
        void Set(Type type, object target);
        bool Has<T>();
        bool Has(Type type);
    }
}