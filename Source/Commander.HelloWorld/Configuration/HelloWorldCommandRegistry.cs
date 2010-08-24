using Commander.Commander;
using Commander.HelloWorld.Domain;
using Commander.Registration.Dsl;

namespace Commander.HelloWorld.Configuration
{
    public class HelloWorldCommandRegistry : CommandRegistry
    {
        public HelloWorldCommandRegistry()
        {
            Applies
                .ToThisAssembly();

            Entities
                .IncludedTypesInNamespaceContaining<EntityMarker>()
                .Exclude<EntityMarker>();

            Policies
                .ConditionallyWrapCommandChainsWith<MyCommand>(call => call.ParentChain().EntityType == typeof (User));
        }
    }

    public class MyCommand : BasicCommand
    {
        protected override DoNext PerformInvoke()
        {
            return base.PerformInvoke();
        }
    }
}