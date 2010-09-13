using System;
using FubuCore.Util;

namespace Commander.Runtime
{
    public class CommandContext : ICommandContext
    {
        private readonly Cache<Type, object> _values = new Cache<Type, object>();
        public CommandContext(IEntityBuilderRegistry registry)
        {
            Set<ICompilationContext>(new NulloCompilationContext());
            _values.OnMissing = (type => Get<ICompilationContext>().Get<IEntityBuilder>(registry.BuilderFor(type)).Build());
            _values[typeof(ICommandContext)] = this;
        }

        public T Get<T>() where T : class
        {
            return Get(typeof (T)) as T;
        }

        public object Get(Type type)
        {
            return _values[type];
        }

        public void Set<T>(T target) where T : class
        {
            Set(typeof (T), target);
        }

        public virtual void Set(Type type, object target)
        {
            _values[type] = target;
        }

        public bool Has<T>()
        {
            return Has(typeof (T));
        }

        public bool Has(Type type)
        {
            return _values.Has(type);
        }
    }
}