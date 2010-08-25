using Commander.Commander;
using Commander.Runtime;

namespace Commander.Commands
{
    public class EntityRequestCommand : BasicCommand
    {
        private readonly ICommandContext _commandContext;
        private readonly EntityRequest _entityRequest;

        public EntityRequestCommand(EntityRequest entityRequest, ICommandContext commandContext)
        {
            _entityRequest = entityRequest;
            _commandContext = commandContext;
        }

        protected override DoNext PerformInvoke()
        {
            _commandContext.Set(_entityRequest);
            return DoNext.Continue;
        }
    }
}