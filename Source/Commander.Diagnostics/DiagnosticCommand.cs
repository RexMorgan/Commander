using Commander.Commands;

namespace Commander.Diagnostics
{
    public class DiagnosticCommand : ICommand
    {
        private readonly ICommanderDebugReport _report;
        private readonly ICommanderDebugDetector _detector;
        private readonly ICommand _inner;

        public DiagnosticCommand(ICommanderDebugReport report, ICommanderDebugDetector detector, ICommand inner)
        {
            _report = report;
            _detector = detector;
            _inner = inner;
        }

        public void Execute()
        {
            _inner.Execute();

            if (!_detector.IsDebugCall())
            {
                return;
            }

            _report.MarkFinished();

            //var debugWriter = new DebugWriter(_report, _urls);
            //var outputWriter = new HttpResponseOutputWriter();

            //outputWriter.Write(MimeType.Html.ToString(), debugWriter.Write().ToString());
        }
    }
}