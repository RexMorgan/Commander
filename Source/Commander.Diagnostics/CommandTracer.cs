using System;
using Commander.Commands;

namespace Commander.Diagnostics
{
    public class CommandTracer : ICommand
    {
        private readonly ICommanderDebugReport _report;
        private readonly ICommanderDebugDetector _debugDetector;
        private readonly ICommand _inner;

        public CommandTracer(ICommanderDebugReport report, ICommanderDebugDetector debugDetector, ICommand inner)
        {
            _report = report;
            _debugDetector = debugDetector;
            _inner = inner;
        }

        public void Execute()
        {
            _report.StartCommand(_inner);

            try
            {
                _inner.Execute();
            }
            catch (Exception ex)
            {
                _report.MarkException(ex);
                if (!_debugDetector.IsDebugCall())
                {
                    throw;
                }
            }

            _report.EndCommand();
        }
    }
}