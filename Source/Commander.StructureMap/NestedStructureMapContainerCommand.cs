using System;
using Commander.Commander;
using FubuCore.Binding;
using StructureMap;
using StructureMap.Pipeline;

namespace Commander.StructureMap
{
    public class NestedStructureMapContainerCommand : ICommand
    {
        private readonly ExplicitArguments _arguments;
        private readonly Guid _behaviorId;
        private readonly IContainer _container;

        public NestedStructureMapContainerCommand(IContainer container, ServiceArguments arguments, Guid behaviorId)
        {
            _container = container;
            _arguments = arguments.ToExplicitArgs();
            _behaviorId = behaviorId;
        }


        public void Execute()
        {
            using (IContainer nested = _container.GetNestedContainer())
            {
                var command = nested.GetInstance<ICommand>(_arguments, _behaviorId.ToString());
                command.Execute();
            }
        }
    }
}