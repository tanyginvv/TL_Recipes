namespace Recipes.Application.Repositories
{
    public interface IAddedRepository<TEntity> where TEntity : class
    {
        Task AddAsync( TEntity entety );
    }
}
