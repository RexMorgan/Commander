using Commander.StructureMap;
using NUnit.Framework;
using Rhino.Mocks;
using StructureMap;

namespace Commander.Tests.Scenarios
{
    [TestFixture]
    public class ManualGraphTester
    {
        [Test]
        public void wrapping_commands_get_executed()
        {
            var dependency = MockRepository.GenerateMock<IDependency>();
            var container = new Container(x => x.For<IDependency>().Use(dependency));
            var facility = new StructureMapContainerFacility(container);
            CommanderFactory.Initialize(facility, registry =>
                                                      {
                                                          registry
                                                              .Applies
                                                              .ToThisAssembly();

                                                          registry
                                                              .Entities
                                                              .IncludeType<User>();

                                                          registry
                                                              .Policies
                                                              .WrapCommandChainsWith<DummyCommand>(cmd => cmd.Execute());
                                                      });

            dependency.Expect(d => d.Mark());
            dependency.Replay();

            CommanderFactory
                .Invoker
                .ForNew(new MyUserCommand());

            dependency.VerifyAllExpectations();
        }

        #region Nested Type: MyUserCommand
        public class MyUserCommand : IDomainCommand<User>
        {
            public void Execute(User entity)
            {
            }
        }
        #endregion

        #region Nested Type: User
        public class User
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
        #endregion

        #region Nested Type: DummyCommand
        public interface IDependency
        {
            void Mark();
        }
        public class DummyCommand
        {
            private readonly IDependency _dependency;

            public DummyCommand(IDependency dependency)
            {
                _dependency = dependency;
            }

            public void Execute()
            {
                _dependency.Mark();
            }
        }
        #endregion
    }
}