using Recipes.Application.Interfaces;
using Recipes.Application.Repositories.BasicRepositories;
using Recipes.Domain.Entities;

namespace Recipes.Application.Repositories
{
    public interface IRecipeRepository :
        IAddEntityRepository<Recipe>,
        IDeleteEntityRepository<Recipe>
    {
        Task<List<Recipe>> GetRecipesAsync( IEnumerable<IFilter<Recipe>> filters );
        Task<Recipe> GetByIdAsync( int id );
    }
}