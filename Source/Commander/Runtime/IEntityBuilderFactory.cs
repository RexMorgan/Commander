using Commander.Registration.Graph;

namespace Commander.Runtime
{
    public interface IEntityBuilderFactory
    {
        IEntityBuilder Build(ObjectDef builderDef);
    }
}