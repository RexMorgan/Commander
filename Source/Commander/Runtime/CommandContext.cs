using System;
using FubuCore.Util;

namespace Commander.Runtime
{
    public class CommandContext : ICommandContext
    {
        private readonly Cache<Type, object> _values = new Cache<Type, object>();
        public CommandContext(IEntityBuilderRegistry registry)
        {
            _values.OnMissing = (type => registry.BuilderFor(type).Build());
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

        public void Set(Type type, object target)
        {
            _values[type] = target;
        }
    }
}