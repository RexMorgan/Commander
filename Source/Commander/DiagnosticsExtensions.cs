using Commander.Diagnostics;

namespace Commander
{
    public static class DiagnosticsExtensions
    {
        public static void IncludeDiagnostics(this CommandRegistry registry)
        {
            registry.UsingObserver(new RecordingConfigurationObserver());
        }
    }
}