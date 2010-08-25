using System.Collections.Generic;
using Commander.Registration.Nodes;
using FubuCore.Util;

namespace Commander.Diagnostics
{
    public class RecordingConfigurationObserver : IConfigurationObserver
    {
        private readonly Cache<CommandNode, IList<string>> _log = new Cache<CommandNode, IList<string>>(c => new List<string>());

        public bool IsRecording
        {
            get { return true; }
        }

        public void RecordCallStatus(CommandNode node, string description)
        {
            _log[node].Add(description);
            LastLogEntry = description;
        }

        public IEnumerable<string> GetLog(CommandNode node)
        {
            return _log[node];
        }

        public string LastLogEntry { get; private set; }
    }
}