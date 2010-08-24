using System;
using Commander.Registration.Graph;
using Commander.Runtime;

namespace Commander.Bootstrapping
{
    public interface IContainerFacility
    {
        ICommandFactory BuildFactory();
        IEntityBuilderFactory BuildEntityBuilderFactory();
        void Register(Type serviceType, ObjectDef objectDef);
    }
}