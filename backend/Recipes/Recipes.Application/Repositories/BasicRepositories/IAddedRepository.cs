namespace Recipes.Application.Repositories.BasicRepositories
{
    public interface IAddedRepository<TEntity> where TEntity : class
    {
        Task AddAsync( TEntity entety );
    }
}
