using System.Collections.Generic;
using Commander.Registration.Nodes;

namespace Commander.Diagnostics
{
    public interface IConfigurationObserver
    {
        bool IsRecording { get; }
        void RecordCallStatus(CommandNode node, string status);
        IEnumerable<string> GetLog(CommandNode node);
    }
}