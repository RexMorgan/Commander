using System;
using Commander.Registration.Graph;
using Commander.Runtime;

namespace Commander.Bootstrapping
{
    public interface IContainerFacility
    {
        ICommandFactory BuildFactory();
        void Register(Type serviceType, ObjectDef objectDef);
    }
}