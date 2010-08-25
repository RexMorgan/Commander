using FubuMVC.Core.Diagnostics;

namespace Commander.Diagnostics
{
    public class CommandStart : ICommandDetails
    {
        public void AcceptVisitor(ICommandDetailsVisitor visitor)
        {

        }

        public bool Equals(BehaviorStart other)
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;

            return obj.GetType() == typeof(BehaviorStart);
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}