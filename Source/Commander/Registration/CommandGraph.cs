using System;
using System.Collections.Generic;
using System.Linq;
using Commander.Diagnostics;
using Commander.Registration.Dsl;
using Commander.Registration.Graph;
using Commander.Registration.Nodes;

namespace Commander.Registration
{
    public class CommandGraph
    {
        private readonly List<CommandChain> _chainsForNew;
        private readonly List<CommandChain> _chainsForExisting;
        private readonly List<CommandRegistry> _registries;
        private readonly TypePool _types;
        public CommandGraph()
        {
            _types = new TypePool();
            _registries = new List<CommandRegistry>();

            _chainsForNew = new List<CommandChain>();
            _chainsForExisting = new List<CommandChain>();
        }

        private readonly IServiceRegistry _services = new ServiceRegistry();

        public IConfigurationObserver Observer { get; private set; }

        public IServiceRegistry Services { get { return _services; } }

        public TypePool Types { get { return _types; } }

        public List<CommandRegistry> Registries { get { return _registries; } }

        public IEnumerable<CommandChain> ChainsForNew { get { return _chainsForNew; } }
        public IEnumerable<CommandChain> ChainsForExisting { get { return _chainsForExisting; } }

        public void EachService(Action<Type, ObjectDef> action)
        {
            _services.Each(action);

            //_chainsForNew.Each(chain => action(typeof(ICommand), chain.ToObjectDef()));
            //_chainsForExisting.Each(chain => action(typeof(ICommand), chain.ToObjectDef()));

            action(typeof(CommandGraph), new ObjectDef
            {
                Value = this
            });
        }

        public void VisitCommands(ICommandVisitor visitor)
        {
            _chainsForNew.Each(visitor.VisitCommand);
            _chainsForExisting.Each(visitor.VisitCommand);
        }

        public void AddChainForNew(CommandChain chain)
        {
            _chainsForNew.Fill(chain);
        }

        public void AddChainForExisting(CommandChain chain)
        {
            _chainsForExisting.Fill(chain);
        }

        public CommandChain ChainForNew<T>()
        {
            return ChainForNew(typeof (T));
        }

        public CommandChain ChainForNew(Type entityType)
        {
            var chain = _chainsForNew.FirstOrDefault(c => c.EntityType == entityType);
            if(chain == null)
            {
                return null;
            }

            return (CommandChain) chain.Clone();
        }

        public CommandChain ChainForExisting<T>()
        {
            return _chainsForExisting.FirstOrDefault(chain => chain.EntityType == typeof(T));
        }
    }
}