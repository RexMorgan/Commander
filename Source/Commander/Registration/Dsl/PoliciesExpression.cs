using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Commander.Commander;
using Commander.Registration.Nodes;
using FubuCore;

namespace Commander.Registration.Dsl
{
    public class PoliciesExpression
    {
        private readonly IList<IConfigurationAction> _actions;

        public PoliciesExpression(IList<IConfigurationAction> actions)
        {
            _actions = actions;
        }

        public PoliciesExpression WrapCommandChainsWith<T>()
            where T : ICommand
        {
            var reason = "wrap with the {0} command".ToFormat(typeof(T).Name);
            var configAction = new VisitCommandsAction(v =>
                                                        {
                                                            v.Actions += chain => chain.Prepend(new Wrapper(typeof(T)));
                                                        }, reason);

            _actions.Fill(configAction);

            return this;
        }

        public PoliciesExpression WrapCommandChainsWith<T>(Expression<Action<T>> expression)
        {
            var reason = "wrap with the {0} command".ToFormat(typeof(T).Name);
            var configAction = new VisitCommandsAction(v =>
                                                        {
                                                            v.Actions += chain => chain.Prepend(CommandCall.For(expression));
                                                        }, reason);

            _actions.Fill(configAction);

            return this;
        }

        public PoliciesExpression ConditionallyWrapCommandChainsWith<T>(Expression<Func<CommandNode, bool>> filter) 
            where T : ICommand
        {
            var reason = "wrap with the {0} command if [{1}]".ToFormat(typeof(T).Name, filter.Body.ToString());
            var chainFilter = filter.Compile();
            var configAction = new VisitCommandsAction(v =>
                                                           {
                                                               v.Filters += chain => chain.ContainsNode(chainFilter);
                                                               v.Actions += chain => chain.Prepend(new Wrapper(typeof(T)));
                                                           }, reason);

            _actions.Fill(configAction);

            return this;
        }

        private void AddPolicy(Action<CommandGraph> action)
        {
            var policy = new LambdaConfigurationAction(action);
            _actions.Add(policy);
        }

        public PoliciesExpression EnrichCommandsForNewWith<T>(Func<CommandCall, bool> filter) 
            where T : ICommand
        {
            AddPolicy(graph => graph
                                   .ChainsForNew
                                   .SelectMany(chain => chain.Calls)
                                   .Where(filter)
                                   .Each(call => call.AddAfter(Wrapper.For<T>())));

            return this;
        }

        public PoliciesExpression EnrichCommandsForExistingWith<T>(Func<CommandCall, bool> filter)
            where T : ICommand
        {
            AddPolicy(graph => graph
                                   .ChainsForExisting
                                   .SelectMany(chain => chain.Calls)
                                   .Where(filter)
                                   .Each(call => call.AddAfter(Wrapper.For<T>())));

            return this;
        }

        public PoliciesExpression Add(IConfigurationAction alteration)
        {
            _actions.Fill(alteration);
            return this;
        }

        public PoliciesExpression Add<T>() 
            where T : IConfigurationAction, new()
        {
            if (_actions.Any(x => x is T))
            {
                return this;
            }

            return Add(new T());
        }
    }
}