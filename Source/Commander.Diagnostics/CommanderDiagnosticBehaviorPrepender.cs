using System.Collections.Generic;
using System.Linq;
using Commander.Registration;
using Commander.Registration.Nodes;

namespace Commander.Diagnostics
{
    public class CommanderDiagnosticBehaviorPrepender : IConfigurationAction
    {
        public void Configure(CommandGraph graph)
        {
            graph.ChainsForNew.Each(c => ModifyChain(c, graph.Observer));
            graph.ChainsForExisting.Each(c => ModifyChain(c, graph.Observer));
        }

        private static void ModifyChain(CommandChain chain, IConfigurationObserver observer)
        {
            chain.Calls.Each(c => observer.RecordCallStatus(c, "Wrapping with diagnostic tracer and behavior"));
            chain.ToArray().Each(node => node.AddBefore(Wrapper.For<CommandTracer>()));
            chain.Prepend(new Wrapper(typeof(DiagnosticCommand)));
        }
    }
}