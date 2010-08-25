using Commander.Runtime;

namespace Commander.Diagnostics
{
    public interface ICommanderDebugDetector
    {
        bool IsDebugCall();
    }

    public class CommanderDebugDetector : ICommanderDebugDetector
    {
        private readonly ICommandContext _context;

        public CommanderDebugDetector(ICommandContext context)
        {
            _context = context;
        }

        public bool IsDebugCall()
        {
            return _context.Has<CommanderDebugDetector>();
        }
    }
}