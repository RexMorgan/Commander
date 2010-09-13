using System;
using Commander.Registration.Graph;
using Commander.Runtime;

namespace Commander.Bootstrapping
{
    public interface ICommanderContainer
    {
        ICompiler BuildCompiler();
        void Register(Type serviceType, ObjectDef objectDef);
    }
}