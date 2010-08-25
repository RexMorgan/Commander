using System;
using Commander.Runtime;

namespace Commander.Diagnostics
{
    public class RecordingCommandContext : CommandContext
    {
        private readonly ICommanderDebugReport _report;
        
        public RecordingCommandContext(IEntityBuilderRegistry registry, ICommanderDebugReport report) 
            : base(registry)
        {
            _report = report;
        }

        public override void Set(Type type, object target)
        {
            _report.AddDetails(new SetValueReport
                                   {
                                       Type = type,
                                       Value = target
                                   });
            base.Set(type, target);
        }
    }
}