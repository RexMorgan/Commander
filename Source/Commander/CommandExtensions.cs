using Commander.Registration.Nodes;
using FubuCore.Reflection;

namespace Commander
{
    public static class CommandExtensions
    {
        public static CommandCall ToCommandCall<TEntity>(this IDomainCommand<TEntity> command)
            where TEntity : class
        {
            return new CommandCall(command.GetType(), 
                ReflectionHelper.GetMethod<IDomainCommand<TEntity>>(cmd => cmd.Execute(null)));
        }
    }
}