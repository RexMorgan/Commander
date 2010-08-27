using Commander.Registration;
using Commander.Registration.Dsl;
using Commander.Runtime;

namespace Commander.Diagnostics
{
    public class CommanderDiagnosticsPackage : IRegistryModification
    {
        public void Modify(CommandRegistry registry)
        {
            registry.Services(x =>
                                  {
                                      x.ReplaceService<ICommanderDebugReport, CommanderDebugReport>();
                                      x.ReplaceService<ICommanderDebugDetector, CommanderDebugDetector>();
                                  });
        }
    }
}