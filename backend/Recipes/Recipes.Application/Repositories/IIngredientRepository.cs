using Recipes.Application.Repositories.BasicRepositories;
using Recipes.Domain.Entities;

namespace Recipes.Application.Repositories;

public interface IIngredientRepository :
    IAddEntityRepository<Ingredient>,
    IDeleteEntityRepository<Ingredient>
{
    Task<IReadOnlyList<Ingredient>> GetByRecipeIdAsync( int recipeId );
    Task<Ingredient> GetByIdAsync( int id );
}