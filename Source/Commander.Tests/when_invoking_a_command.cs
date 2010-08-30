using System;
using Commander.Commands;
using Commander.Registration.Nodes;
using Commander.Runtime;
using NUnit.Framework;
using Rhino.Mocks;

namespace Commander.Tests
{
    [TestFixture]
    public class when_invoking_a_command : InteractionContext<CommandInvoker>
    {
        private Action<ICommandContext> _configure;
        private CommandCall _targetCommand;

        protected override void BeforeEach()
        {
            _configure = ctx => { };
            _targetCommand = new TestCommand().ToCommandCall();
            MockFor<ICompiledCommand>()
                .Expect(cmd => cmd.Command)
                .Return(MockFor<ICommand>());

            MockFor<ICompiledCommand>()
                .Expect(cmd => cmd.Context)
                .Return(MockFor<ICommandContext>());
        }

        [Test]
        public void the_compiler_is_invoked()
        {
            var executed = false;
            Compiler compiler = (g, ctx, call) =>
                                                {
                                                    executed = true;
                                                    return MockFor<ICompiledCommand>();
                                                };

            ClassUnderTest.Invoke<User>(_configure, _targetCommand, compiler);

            executed.ShouldBeTrue();
        }

        #region Nested Type: TestCommand
        public class TestCommand : IDomainCommand<User>
        {
            public void Execute(User entity)
            {
            }
        }
        #endregion
    }
}