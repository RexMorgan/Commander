using System.Collections.Generic;
using System.Linq;
using Commander.Commands;
using Commander.Diagnostics;
using Commander.Registration.Nodes;

namespace Commander.Registration
{
    public class InvocationTracerPrepender : IConfigurationAction
    {
        public void Configure(CommandGraph graph)
        {
            graph.ChainsForNew.Each(c => ModifyChain(c, graph.Observer));
            graph.ChainsForExisting.Each(c => ModifyChain(c, graph.Observer));
        }

        private static void ModifyChain(CommandChain chain, IConfigurationObserver observer)
        {
            chain.Calls.Each(c => observer.RecordCallStatus(c, "Wrapping with invocation tracer"));
            chain.ToArray().Each(node => node.AddBefore(new Wrapper(typeof(InvocationTracer<>).MakeGenericType(chain.EntityType))));
        }
    }
}