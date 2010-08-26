using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Commander.Commands;
using Commander.Registration.Graph;

namespace Commander.Registration.Nodes
{
    public abstract class CommandNode : IEnumerable<CommandNode>
    {
        private readonly Guid _uniqueId = Guid.NewGuid();
        private CommandNode _next;
        public virtual Guid UniqueId { get { return _uniqueId; } }
        public abstract CommandCategory Category { get; }


        public CommandNode Next
        {
            get { return _next; }
            protected set
            {
                _next = value;
                if (value != null) value.Previous = this;
            }
        }

        public CommandNode Previous { get; protected set; }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public virtual IEnumerator<CommandNode> GetEnumerator()
        {
            if (Next != null)
            {
                yield return Next;

                foreach (CommandNode node in Next)
                {
                    yield return node;
                }
            }
        }

        public CommandChain ParentChain()
        {
            if (this is CommandChain) return (CommandChain)this;

            if (Previous == null) return null;

            if (Previous is CommandChain) return (CommandChain)Previous;

            return Previous.ParentChain();
        }

        public virtual ObjectDef ToObjectDef()
        {
            ObjectDef objectDef = toObjectDef();
            objectDef.Name = UniqueId.ToString();

            return objectDef;
        }

        protected ObjectDef toObjectDef()
        {
            ObjectDef objectDef = buildObjectDef();
            if (Next != null)
            {
                var dependency = new ConfiguredDependency
                {
                    Definition = Next.ToObjectDef(),
                    DependencyType = typeof(ICommand)
                };

                objectDef.Dependencies.Add(dependency);
            }

            return objectDef;
        }

        protected abstract ObjectDef buildObjectDef();

        public void AddAfter(CommandNode node)
        {
            CommandNode next = Next;
            Next = node;
            node.Next = next;
        }

        public void AddBefore(CommandNode newNode)
        {
            if (Previous != null) Previous.Next = newNode;
            newNode.Next = this;
        }

        public Wrapper WrapWith<T>() where T : ICommand
        {
            return WrapWith(typeof(T));
        }

        public Wrapper WrapWith(Type behaviorType)
        {
            var wrapper = new Wrapper(behaviorType);
            AddBefore(wrapper);

            return wrapper;
        }

        public virtual void AddToEnd(CommandNode node)
        {
            CommandNode last = this.LastOrDefault() ?? this;
            last.Next = node;
        }

        public virtual void Remove()
        {
            if (Previous == null)
            {
                Next.Previous = null;
            }
            else
            {
                Previous.Next = Next;
            }
            Previous = null;
            Next = null;
        }

        public virtual void ReplaceWith(CommandNode newNode)
        {
            if (Previous != null)
            {
                Previous.Next = newNode;
            }
            newNode.Next = Next;
            Previous = null;
            Next = null;
        }

        public virtual CommandNode Clone()
        {
            var copy = Copy();
            if(Next != null)
            {
                copy.Next = Next.Clone();
            }

            return copy;
        }

        public abstract CommandNode Copy();
    }
}