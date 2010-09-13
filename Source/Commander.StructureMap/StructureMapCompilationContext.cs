using System;
using Commander.Commands;
using Commander.Registration.Graph;
using Commander.Runtime;
using StructureMap;

namespace Commander.StructureMap
{
    public class StructureMapCompilationContext : ICompilationContext
    {
        private readonly IContainer _container;
        private ICommand _command;

        public StructureMapCompilationContext(IContainer container, ICommand command)
        {
            _container = container;
            _command = command;
        }

        public void Dispose()
        {
            _command = null;
            if(_container != null)
            {
                _container.Dispose();
            }
        }

        public ICommand Command
        {
            get { return _command; }
        }

        public ICommandContext Context
        {
            get { return _container.GetInstance<ICommandContext>(); }
        }

        public object Get(Type type)
        {
            return _container.GetInstance(type);
        }

        public T Get<T>(ObjectDef def)
            where T : class
        {
            return _container.GetInstance(def.Type) as T;
        }

        public T Get<T>()
        {
            return _container.GetInstance<T>();
        }
    }
}