using Recipes.Application.Paginator;
using Recipes.Domain.Entities;

namespace Recipes.Application.Repositories
{
    public interface IRecipeRepository
    {
        Task AddAsync( Recipe recipe );
        Task DeleteAsync( int id );
        Task<IReadOnlyList<Recipe>> GetAllAsync( PaginationFilter paginationFilter );
        Task<IReadOnlyList<Recipe>> GetFilteredRecipesAsync( IEnumerable<string> searchTerms, PaginationFilter paginationFilter );
        Task<Recipe> GetByIdAsync( int id );
    }
}
