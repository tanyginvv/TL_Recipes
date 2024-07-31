using Recipes.Domain.Entities;

namespace Recipes.Application.Repositories
{
    public interface IIngredientRepository :
        IAddedRepository<Ingredient>,
        IRemovableRepository<Ingredient>
    {
        Task<IReadOnlyList<Ingredient>> GetByRecipeIdAsync( int recipeId );
        Task<Ingredient> GetByIdAsync( int id );
    }
}