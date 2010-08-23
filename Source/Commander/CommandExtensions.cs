using System.Reflection;
using Commander.Registration.Nodes;

namespace Commander
{
    public static class CommandExtensions
    {
        public static CommandCall ToCommandCall<TEntity>(this IDomainCommand<TEntity> command)
            where TEntity : class
        {
            var cmdType = command.GetType();
            var method = cmdType.GetMethod("Execute", BindingFlags.Instance | BindingFlags.Public);

            return new CommandCall(cmdType, method);
        }
    }
}