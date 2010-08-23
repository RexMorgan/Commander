using FubuCore.Binding;
using Microsoft.Practices.ServiceLocation;
using StructureMap.Configuration.DSL;

namespace Commander.StructureMap
{
    public class StructureMapCommanderRegistry : Registry
    {
        public StructureMapCommanderRegistry()
        {
            For<AggregateDictionary>().Use(new AggregateDictionary());
            For<IServiceLocator>().Use<StructureMapServiceLocator>();
        }
    }
}