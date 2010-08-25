using System;
using Commander.Runtime;

namespace Commander
{
    public interface ICommandInvoker
    {
        void ForNew<TEntity>(IDomainCommand<TEntity> command)
            where TEntity : class;

        void ForExisting<TEntity>(Action<EntityRequest> action, IDomainCommand<TEntity> command)
            where TEntity : class;
    }

    public static class InvocationExtensions
    {
        public static void ForNew<TEntity>(this ICommandInvoker invoker, Action<TEntity> action)
            where TEntity : class
        {
            invoker.ForNew(new LambdaDomainCommand<TEntity>(action));
        }
    }

    public class UpdateEndpoint
    {
        private readonly ICommandInvoker _invoker;

        public UpdateEndpoint(ICommandInvoker invoker)
        {
            _invoker = invoker;
        }

        public AjaxResponse Post(UpdateUserInputModel inputModel)
        {
            _invoker.ForExisting(request => { request.EntityId = inputModel.UserId; }, new UpdateUserCommand(inputModel));

            return null;
        }
    }

    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
    }
    
    public class AjaxResponse { }
    
    public class UpdateUserInputModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
    }

    public class UpdateUserCommand : IDomainCommand<User>
    {
        private readonly UpdateUserInputModel _model;

        public UpdateUserCommand(UpdateUserInputModel model)
        {
            _model = model;
        }

        public void Execute(User entity)
        {
            entity.FirstName = _model.FirstName;
        }
    }
}