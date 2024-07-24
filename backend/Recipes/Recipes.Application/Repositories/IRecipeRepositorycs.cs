using Recipes.Application.Paginator;
using Recipes.Domain.Entities;

namespace Recipes.Application.Repositories
{
    public interface IRecipeRepository :
        IAddedRepository<Recipe>,
        IRemovableRepository<Recipe>
    {
        Task<IReadOnlyList<Recipe>> GetAllAsync( PaginationFilter paginationFilter );
        Task<IReadOnlyList<Recipe>> GetFilteredRecipesAsync( IEnumerable<string> searchTerms, PaginationFilter paginationFilter );
        Task<Recipe> GetByIdAsync( int id );
    }
}
