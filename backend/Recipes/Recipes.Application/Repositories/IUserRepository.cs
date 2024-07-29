
using Recipes.Domain.Entities;

namespace Recipes.Application.Repositories
{
    public interface IUserRepository :
        IAddedRepository<User>,
        IRemovableRepository<User>,
        ISearchRepository<User>
    {
        Task<User> GetByIdAsync( int id );
        Task<User> GetByLoginAsync( string login );
        Task<IReadOnlyList<Recipe>> GetRecipesAsync( int id );
    }
}
