using Recipes.Domain.Entities;

namespace Recipes.Application.Repositories
{
    public interface IRecipeRepository
    {
        Task AddAsync( Recipe recipe );
        Task DeleteAsync( int id );
        Task<IReadOnlyList<Recipe>> GetAllAsync();
        Task<IReadOnlyList<Recipe>> GetFilteredRecipesAsync( IEnumerable<string> searchTerms );
        Task<Recipe> GetByIdAsync( int id );
        Task UpdateAsync( Recipe recipe );
    }
}