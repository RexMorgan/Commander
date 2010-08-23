using System.Collections.Generic;
using Commander.Registration.Nodes;
using FubuCore.Util;

namespace Commander.Diagnostics
{
    public class RecordingConfigurationObserver : IConfigurationObserver
    {
        private readonly Cache<CommandCall, IList<string>> _log = new Cache<CommandCall, IList<string>>(c => new List<string>());

        public bool IsRecording
        {
            get { return true; }
        }

        public void RecordCallStatus(CommandCall call, string description)
        {
            _log[call].Add(description);
            LastLogEntry = description;
        }

        public IEnumerable<string> GetLog(CommandCall call)
        {
            return _log[call];
        }

        public string LastLogEntry { get; private set; }
    }
}