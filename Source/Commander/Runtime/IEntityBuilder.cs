namespace Commander.Runtime
{
    public interface IEntityBuilder
    {
        object Build();
    }

    public interface IEntityBuilder<TEntity> : IEntityBuilder
    {
    }
}