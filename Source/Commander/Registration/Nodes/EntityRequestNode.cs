using Commander.Commands;

namespace Commander.Registration.Nodes
{
    public class EntityRequestNode : Wrapper
    {
        public EntityRequestNode()
            : base(typeof(EntityRequestCommand))
        {
        }
    }
}