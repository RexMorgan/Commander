using System;
using Commander.Commands;
using Commander.Registration.Graph;

namespace Commander.Registration.Nodes
{
    public class Wrapper : CommandNode
    {
        private readonly ObjectDef _objectDef;

        public Wrapper(Type commandType)
        {
            _objectDef = new ObjectDef
            {
                Type = commandType
            };
        }

        public Type CommandType { get { return _objectDef.Type; } }

        public override CommandCategory Category { get { return CommandCategory.Wrapper; } }

        public static Wrapper For<T>() where T : ICommand
        {
            return new Wrapper(typeof(T));
        }

        protected override ObjectDef buildObjectDef()
        {
            return _objectDef;
        }

        public override string ToString()
        {
            return "Wrapped by " + _objectDef.Type.FullName;
        }

        public override CommandNode Copy()
        {
            return new Wrapper(CommandType);
        }
    }
}