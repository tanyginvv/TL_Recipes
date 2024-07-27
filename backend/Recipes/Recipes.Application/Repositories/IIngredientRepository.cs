using Recipes.Domain.Entities;

namespace Recipes.Application.Repositories
{
    public interface IIngredientRepository
    {
        Task<IReadOnlyList<Ingredient>> GetByRecipeIdAsync( int recipeId );
        Task<Ingredient> GetByIdAsync( int id );
        Task AddAsync( Ingredient ingredient );
        Task UpdateAsync( Ingredient ingredient );
        Task DeleteAsync( int id );
    }
}