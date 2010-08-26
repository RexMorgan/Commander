using System;
using Commander.Commands;
using FubuCore.Binding;
using StructureMap;
using StructureMap.Pipeline;

namespace Commander.StructureMap
{
    public class StructureMapContainerCommand : ICommand
    {
        private readonly ExplicitArguments _arguments;
        private readonly Guid _behaviorId;
        private readonly IContainer _container;

        public StructureMapContainerCommand(IContainer container, ServiceArguments arguments, Guid behaviorId)
        {
            _container = container;
            _arguments = arguments.ToExplicitArgs();
            _behaviorId = behaviorId;
        }


        public void Execute()
        {
            var command = _container.GetInstance<ICommand>(_arguments, _behaviorId.ToString());
            command.Execute();
        }
    }
}