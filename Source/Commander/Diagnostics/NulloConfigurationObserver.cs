using System.Collections.Generic;
using Commander.Registration.Nodes;

namespace Commander.Diagnostics
{
    public class NulloConfigurationObserver : IConfigurationObserver
    {
        public bool IsRecording
        {
            get { return false; }
        }

        public void RecordCallStatus(CommandNode node, string status)
        {
        }

        public void RecordStatus(string status)
        {
        }

        public IEnumerable<string> GetLog(CommandNode node)
        {
            yield break;
        }
    }
}