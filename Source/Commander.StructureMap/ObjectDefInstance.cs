using Commander.Registration.Graph;
using StructureMap.Pipeline;

namespace Commander.StructureMap
{
    public class ObjectDefInstance : ConfiguredInstance, IDependencyVisitor
    {
        public ObjectDefInstance(ObjectDef definition)
            : base(definition.Type)
        {
            definition.AcceptVisitor(this);
            Name = definition.Name;
        }

        void IDependencyVisitor.Value(ValueDependency dependency)
        {
            Child(dependency.DependencyType).Is(dependency.Value);
        }

        void IDependencyVisitor.Configured(ConfiguredDependency dependency)
        {
            var child = new ObjectDefInstance(dependency.Definition);
            Child(dependency.DependencyType).Is(child);
        }
    }
}