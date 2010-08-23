using System.Collections.Generic;
using Commander.Registration.Nodes;

namespace Commander.Diagnostics
{
    public interface IConfigurationObserver
    {
        bool IsRecording { get; }
        void RecordCallStatus(CommandCall call, string status);
        IEnumerable<string> GetLog(CommandCall call);
    }
}