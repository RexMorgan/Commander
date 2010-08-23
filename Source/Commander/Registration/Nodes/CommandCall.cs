using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Commander.Commander;
using Commander.Registration.Graph;
using FubuCore;
using FubuCore.Reflection;

namespace Commander.Registration.Nodes
{
    public class CommandCall : CommandNode
    {
        public CommandCall(Type handlerType, MethodInfo method)
        {
            HandlerType = handlerType;
            Method = method;
            Next = null;
        }

        public Type HandlerType { get; private set; }
        public MethodInfo Method { get; private set; }

        public bool HasInput { get { return Method.GetParameters().Length > 0; } }

        private bool hasReturn { get { return Method.ReturnType != typeof(void); } }
        public override CommandCategory Category { get { return CommandCategory.Call; } }
        public string Description { get { return "{0}.{1}({2}) : {3}".ToFormat(HandlerType.Name, Method.Name, getInputParameters(), hasReturn ? Method.ReturnType.Name : "void"); } }

        private string getInputParameters()
        {
            if (!HasInput) return "";

            return Method.GetParameters().Select(p => "{0} {1}".ToFormat(p.ParameterType.Name, p.Name)).Join(", ");
        }

        public static CommandCall For<T>(Expression<Action<T>> expression)
        {
            MethodInfo method = ReflectionHelper.GetMethod(expression);
            return new CommandCall(typeof(T), method);
        }

        public static CommandCall For<T>(Expression<Func<T, object>> expression)
        {
            MethodInfo method = ReflectionHelper.GetMethod(expression);
            return new CommandCall(typeof(T), method);
        }

        //public bool Returns<T>()
        //{
        //    return OutputType().CanBeCastTo<T>();
        //}

        protected override ObjectDef buildObjectDef()
        {
            Validate();

            return new ObjectDef
            {
                Dependencies = new List<IDependency>
                {
                    createLambda()
                },
                Type = determineHandlerType()
            };
        }

        public void Validate()
        {
            if (hasReturn && Method.ReturnType.IsValueType)
            {
                throw new CommanderException(1001,
                                        "The return type of command '{0}' must be void (no return type).",
                                        Description);
            }

            var parameters = Method.GetParameters();
            if (parameters != null && parameters.Length > 1)
            {
                throw new CommanderException(1002,
                                        "Command '{0}' has more than one input parameter. A command must either have no input parameters, or it must have one that is a reference type (class).",
                                        Description);
            }

            if (HasInput && InputType().IsValueType)
            {
                throw new CommanderException(1003,
                                        "The type of the input parameter of command '{0}' is a value type (struct). A command must either have no input parameters, or it must have one that is a reference type (class).",
                                        Description);
            }
        }

        private Type determineHandlerType()
        {
            if (HasInput && !hasReturn)
            {
                return typeof(OneInZeroOutCommandInvoker<,>)
                    .MakeGenericType(HandlerType, Method.GetParameters().First().ParameterType);
            }

            if (!HasInput && !hasReturn)
            {
                return typeof(ZeroInZeroOutCommandInvoker<>)
                    .MakeGenericType(HandlerType);
            }

            throw new CommanderException(1002,
                "The command '{0}' is invalid. Only methods that support the '1 in 0 out' and '0 in 0 out' patterns are valid here", Description);
        }

        private ValueDependency createLambda()
        {
            object lambda = hasReturn
                                ? FuncBuilder.ToFunc(HandlerType, Method)
                                : FuncBuilder.ToAction(HandlerType, Method);
            return new ValueDependency
            {
                DependencyType = lambda.GetType(),
                Value = lambda
            };
        }

        //public Type OutputType()
        //{
        //    return Method.ReturnType;
        //}

        public Type InputType()
        {
            return HasInput ? Method.GetParameters().First().ParameterType : null;
        }

        public override string ToString()
        {
            return string.Format("Call {0}", Description);
        }

        public bool Equals(CommandCall other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.HandlerType, HandlerType) && Equals(other.Method, Method);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(CommandCall)) return false;
            return Equals((CommandCall)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((HandlerType != null ? HandlerType.GetHashCode() : 0) * 397) ^
                       (Method != null ? Method.GetHashCode() : 0);
            }
        }

        public override CommandNode Copy()
        {
            return new CommandCall(HandlerType, Method);
        }
    }
}