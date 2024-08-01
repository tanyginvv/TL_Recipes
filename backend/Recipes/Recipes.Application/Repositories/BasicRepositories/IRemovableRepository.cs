namespace Recipes.Application.Repositories.BasicRepositories
{
    public interface IRemovableRepository<TEntity> where TEntity : class
    {
        Task Delete( TEntity entity );
    }
}
