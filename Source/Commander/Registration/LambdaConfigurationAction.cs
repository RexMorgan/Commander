using System;

namespace Commander.Registration
{
    public class LambdaConfigurationAction : IConfigurationAction
    {
        private readonly Action<CommandGraph> _action;

        public LambdaConfigurationAction(Action<CommandGraph> action)
        {
            _action = action;
        }

        public void Configure(CommandGraph graph)
        {
            _action(graph);
        }
    }
}