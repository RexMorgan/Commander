using System;
using Commander.Bootstrapping;
using Commander.Registration;
using Commander.Registration.Nodes;
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
            var chain = CommandChain.For<User>();
            chain.Prepend(CommandCall.For<DummyCommand>(cmd => cmd.Execute()));

            var graph = new CommandGraph();
            graph.AddChainForNew(chain);

            var dependency = MockRepository.GenerateMock<IDependency>();

            var container = new Container(x => x.For<IDependency>().Use(dependency));
            var facility = new StructureMapContainerFacility(container);
            container.Configure(x => x.For<IContainerFacility>().Use(facility));

            graph
                .Services
                .SetServiceIfNone<ICommandContext, CommandContext>();
            graph
                .Services
                .SetServiceIfNone<IEntityBuilderRegistry, EntityBuilderRegistry>();

            graph.EachService(facility.Register);

            var compiler = new CommandCompiler(facility);
            var invoker = new CommandInvoker(graph, compiler);

            dependency.Expect(d => d.Mark());
            dependency.Replay();

            invoker.ForNew(new MyUserCommand());

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