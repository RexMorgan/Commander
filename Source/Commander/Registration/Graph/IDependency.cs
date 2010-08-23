using System;

namespace Commander.Registration.Graph
{
    public interface IDependency
    {
        Type DependencyType { get; }
        void AcceptVisitor(IDependencyVisitor visitor);
    }
}