using System;
using System.Collections.Generic;
using Commander.Commands;

namespace Commander.Diagnostics
{
    public interface ICommanderDebugReport
    {
        CommandReport StartCommand(ICommand command);
        void EndCommand();
        void AddDetails(ICommandDetails details);
        void MarkException(Exception exception);
        DateTime Time { get; set; }
        double ExecutionTime { get; }
        void MarkFinished();

        IEnumerable<CommandStep> Steps { get; }
    }
}