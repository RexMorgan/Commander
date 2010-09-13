using Commander.Runtime;
using Commander.StructureMap;
using Commander.Tests.Scenarios;
using NUnit.Framework;
using Rhino.Mocks;
using StructureMap;

namespace Commander.Tests
{
    [TestFixture]
    public class EntityBuilderTester
    {
        [Test]
        public void dependencies_are_resolved_for_entity_builders()
        {
            var dependency = MockRepository.GenerateMock<IDependency>();
            var container = new Container(x => x.For<IDependency>().Use(dependency));
            var facility = new StructureMapContainerFacility(container);

            CommanderFactory.Initialize(facility, new EntityBuilderTesterRegistry());
            
            dependency.Expect(d => d.Mark());
            dependency.Replay();

            CommanderFactory.Invoker.ForNew<User>(ctx => { });

            dependency.VerifyAllExpectations();
        }
    }

    public class EntityBuilderTesterRegistry : CommandRegistry
    {
        public EntityBuilderTesterRegistry()
        {
            Applies
                .ToThisAssembly();

            Entities
                .IncludeType<User>();

            EntityBuilders
                .Exclude<ManualGraphTester.UserBuilder>()
                .RegisterAllAvailable();
        }
    }

    public class UserBuilderTester : IEntityBuilder<User>
    {
        private readonly IDependency _dependency;

        public UserBuilderTester(IDependency dependency)
        {
            _dependency = dependency;
        }

        public object Build()
        {
            _dependency.Mark();
            return new User(1);
        }
    }
}