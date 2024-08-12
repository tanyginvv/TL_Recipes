namespace Recipes.Application.Repositories.BasicRepositories;

public interface IDeleteEntityRepository<TEntity> where TEntity : class
{
    Task Delete( TEntity entity );
}