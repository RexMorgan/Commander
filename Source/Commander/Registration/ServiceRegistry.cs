using System;
using System.Collections.Generic;
using System.Linq;
using Commander.Registration.Graph;
using FubuCore.Util;

namespace Commander.Registration
{
    public class ServiceRegistry : IServiceRegistry
    {
        private readonly Cache<Type, List<ObjectDef>> _services =
            new Cache<Type, List<ObjectDef>>(t => new List<ObjectDef>());

        public void SetServiceIfNone<TService, TImplementation>() where TImplementation : TService
        {
            fill(typeof(TService), new ObjectDef(typeof(TImplementation)));
        }

        public void SetServiceIfNone<TService>(TService value)
        {
            fill(typeof(TService), new ObjectDef
            {
                Value = value
            });
        }

        public void SetServiceIfNone(Type interfaceType, Type concreteType)
        {
            fill(interfaceType, new ObjectDef(concreteType));
        }

        public ObjectDef AddService<TService, TImplementation>() where TImplementation : TService
        {
            var objectDef = new ObjectDef(typeof(TImplementation));
            _services[typeof(TService)].Add(objectDef);

            return objectDef;
        }

        public void ReplaceService<TService, TImplementation>() where TImplementation : TService
        {
            _services[typeof(TService)].Clear();
            AddService<TService, TImplementation>();
        }

        public void ReplaceService<TService>(TService value)
        {
            _services[typeof(TService)].Clear();
            AddService(value);
        }

        public void AddService<TService>(TService value)
        {
            var objectDef = new ObjectDef
            {
                Value = value
            };
            _services[typeof(TService)].Add(objectDef);
        }

        public ObjectDef DefaultServiceFor<TService>()
        {
            return _services[typeof(TService)].FirstOrDefault();
        }

        public IEnumerable<ObjectDef> ServicesFor<TService>()
        {
            return _services[typeof(TService)];
        }

        public void Each(Action<Type, ObjectDef> action)
        {
            _services.Each((t, list) => list.Each(def => action(t, def)));
        }

        public IEnumerable<T> FindAllValues<T>()
        {
            foreach (ObjectDef def in _services[typeof(T)])
            {
                if (def.Value != null)
                {
                    yield return (T)def.Value;
                }
            }
        }

        public void ClearAll<T>()
        {
            _services[typeof(T)].Clear();
        }

        private void fill(Type serviceType, ObjectDef def)
        {
            List<ObjectDef> list = _services[serviceType];
            if (list.Any()) return;

            list.Add(def);
        }

        public static bool ShouldBeSingleton(Type type)
        {
            return type.Name.EndsWith("Cache");
        }
    }
}