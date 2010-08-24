using System.Collections.Generic;
using Commander.Diagnostics;
using Commander.Registration.Nodes;
using FubuCore;
using FubuCore.Util;

namespace Commander.Registration
{
    public class CommandVisitor : ICommandVisitor
    {
        private readonly IConfigurationObserver _observer;
        private readonly string _reasonToVisit;
        private readonly CompositeAction<CommandChain> _actions = new CompositeAction<CommandChain>();
        private readonly CompositePredicate<CommandChain> _filters = new CompositePredicate<CommandChain>();

        public CompositeAction<CommandChain> Actions { get { return _actions; } set { } }
        public CompositePredicate<CommandChain> Filters { get { return _filters; } set { } }

        public CommandVisitor(IConfigurationObserver observer, string reasonToVisit)
        {
            _observer = observer;
            _reasonToVisit = reasonToVisit;
        }

        public void VisitCommand(CommandChain chain)
        {
            if (!_filters.MatchesAll(chain))
            {
                return;
            }

            var matchesDescriptions = _filters.GetDescriptionOfMatches(chain).Join(", ");
            if( matchesDescriptions == string.Empty)
            {
                matchesDescriptions = "(no filters defined)";
            }

            chain.Calls.Each(call => _observer.RecordCallStatus(call,
                                                                "Visiting: {0}. Matched on filters [{1}]".ToFormat
                                                                    (_reasonToVisit, matchesDescriptions)));

            _actions.Do(chain);
        }
    }
}