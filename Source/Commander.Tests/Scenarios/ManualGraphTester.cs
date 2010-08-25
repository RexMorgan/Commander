using System;
using Commander.Diagnostics;
using Commander.Runtime;
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
                                                              .EntityBuilders
                                                              .IncludeTypesClosing(typeof (IEntityBuilder<>));

                                                          registry
                                                              .EntityBuilders
                                                              .RegisterAllAvailable();

                                                          registry
                                                              .Policies
                                                              .WrapCommandChainsWith<DummyCommand>(cmd => cmd.Execute());

                                                          registry.IncludeDiagnostics();
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
                entity.FirstName = "Test";
            }
        }
        #endregion

        #region Nested Type: User
        public class User
        {
            public User(int userId)
            {
                UserId = userId;
            }

            public int UserId { get; private set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
        #endregion

        #region Nested Type: UserBuilder
        public class UserBuilder : IEntityBuilder<User>
        {
            public Type EntityType
            {
                get { return typeof (User); }
            }

            public object Build()
            {
                return new User(1);
            }
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