using System;

namespace Commander.Registration.Graph
{
    public class ValueDependency : IDependency
    {
        public object Value { get; set; }


        public void AcceptVisitor(IDependencyVisitor visitor)
        {
            visitor.Value(this);
        }

        public Type DependencyType { get; set; }

        public override string ToString()
        {
            return string.Format("DependencyType: {1}, Value: {0}", Value, DependencyType);
        }
    }
}