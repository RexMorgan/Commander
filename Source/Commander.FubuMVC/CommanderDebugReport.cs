using System;
using System.Collections;
using System.Collections.Generic;
using Commander.Commander;
using FubuMVC.Core.Diagnostics;

namespace Commander.Diagnostics
{
    public class CommanderDebugReport : TimedReport, IEnumerable<CommandReport>, ICommanderDebugReport
    {
        private readonly IList<CommandReport> _commands = new List<CommandReport>();
        private readonly Stack<CommandReport> _commandStack = new Stack<CommandReport>();
        private readonly IList<CommandStep> _steps = new List<CommandStep>();

        public CommanderDebugReport()
        {
            Time = DateTime.Now;
        }

        public CommandReport StartCommand(ICommand command)
        {
            var report = new CommandReport(command);
            _commands.Add(report);
            _commandStack.Push(report);

            AddDetails(new CommandStart());

            return report;
        }

        public void EndCommand()
        {
            CommandReport report = _commandStack.Pop();
            report.MarkFinished();
        }

        public void AddDetails(ICommandDetails details)
        {
            if (_commandStack.Count == 0)
            {
                return;
            }

            var report = _commandStack.Peek();

            _steps.Add(new CommandStep
                           {
                               Command = report,
                               Details = details
                           });

            report.AddDetail(details);
        }

        public void MarkException(Exception exception)
        {
            var details = new ExceptionReport
                              {
                                  Text = exception.ToString()
                              };

            AddDetails(details);
        }

        public DateTime Time { get; set; }

        public IEnumerable<CommandStep> Steps
        {
            get { return _steps; }
        }

        public IEnumerator<CommandReport> GetEnumerator()
        {
            return _commands.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}