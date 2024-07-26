using Recipes.Application.Interfaces;
using Recipes.Domain.Entities;

namespace Recipes.Application.Repositories
{
    public interface IRecipeRepository :
        IAddedRepository<Recipe>,
        IRemovableRepository<Recipe>
    {
        Task<List<Recipe>> GetRecipesAsync( IEnumerable<IFilter<Recipe>> filters );
        Task<Recipe> GetByIdAsync( int id );
    }
}
