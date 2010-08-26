using Commander.Registration.Dsl;

namespace Commander.Diagnostics
{
    public static class DiagnosticsExtensions
    {
        public static void IncludeDiagnostics(this CommandRegistry registry)
        {
            registry.UsingObserver(new RecordingConfigurationObserver());
            //registry.Import<CommanderDiagnosticsRegistry>();
            registry.Modify<CommanderDiagnosticsPackage>();
            //registry.ApplySystemPolicy<CommanderDiagnosticBehaviorPrepender>();
        }
    }
}