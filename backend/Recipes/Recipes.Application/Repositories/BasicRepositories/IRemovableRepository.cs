namespace Recipes.Application.Repositories
{
    public interface IRemovableRepository<TEntity> where TEntity : class
    {
        Task Delete( TEntity entity );
    }
}
