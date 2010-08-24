using System;

namespace Commander.Registration
{
    public class VisitCommandsAction : IConfigurationAction
    {
        private readonly Action<CommandVisitor> _configureAction;
        private readonly string _reasonToVisit;

        public VisitCommandsAction(Action<CommandVisitor> configureAction, string reasonToVisit)
        {
            _configureAction = configureAction;
            _reasonToVisit = reasonToVisit;
        }

        public void Configure(CommandGraph graph)
        {
            var visitor = new CommandVisitor(graph.Observer, _reasonToVisit);
            _configureAction(visitor);
            graph.VisitCommands(visitor);
        }
    }
}