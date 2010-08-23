namespace Commander
{
    public interface IDomainCommand<TEntity>
        where TEntity : class
    {
        void Execute(TEntity entity);
    }
}