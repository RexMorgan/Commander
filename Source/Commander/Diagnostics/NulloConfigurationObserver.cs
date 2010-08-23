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

        public void RecordCallStatus(CommandCall call, string status)
        {
        }

        public void RecordStatus(string status)
        {
        }

        public IEnumerable<string> GetLog(CommandCall call)
        {
            yield break;
        }
    }
}