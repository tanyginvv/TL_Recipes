namespace Recipes.Application.Repositories.BasicRepositories
{
    public interface IAddEntityRepository<TEntity> where TEntity : class
    {
        Task AddAsync( TEntity entety );
    }
}