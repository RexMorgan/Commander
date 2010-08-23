using System;
using Commander.Registration.Graph;

namespace Commander.Registration.Nodes
{
    public class Placeholder : CommandNode
    {
        public override CommandCategory Category { get { return CommandCategory.Placeholder; } }

        protected override ObjectDef buildObjectDef()
        {
           throw new NotImplementedException();
        }

        public override CommandNode Copy()
        {
            return new Placeholder();
        }
    }
}