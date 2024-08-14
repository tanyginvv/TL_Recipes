using Recipes.Application.Repositories.BasicRepositories;
using Recipes.Domain.Entities;

namespace Recipes.Application.Repositories;

public interface IUserRepository :
    IAddEntityRepository<User>,
    IDeleteEntityRepository<User>,
    ISearchRepository<User>
{
    Task<User> GetByIdAsync( int id );
    Task<User> GetByLoginAsync( string login );
    Task<IReadOnlyList<Recipe>> GetRecipesAsync( int id );
}