using Recipes.Application.Filters;
using Recipes.Application.Repositories.BasicRepositories;
using Recipes.Domain.Entities;

namespace Recipes.Application.Repositories;

public interface IRecipeRepository :
    IAddEntityRepository<Recipe>,
    IDeleteEntityRepository<Recipe>
{
    Task<List<Recipe>> GetRecipesAsync( IEnumerable<IFilter<Recipe>> filters );
    Task<bool> AnyAsync( IEnumerable<IFilter<Recipe>> filters );
    Task<Recipe> GetByIdAsync( int id );
    Task<Recipe> GetRecipeOfDayAsync();
}