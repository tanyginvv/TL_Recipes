using Recipes.Domain.Entities;

namespace Recipes.Infrastructure.Repositories
{
    public interface IIngredientRepository
    {
        Task<IEnumerable<Ingredient>> GetByRecipeIdAsync( int recipeId );
        Task<Ingredient> GetByIdAsync( int id );
        Task AddIngredientAsync( Ingredient ingredient );
        Task UpdateIngredientAsync( Ingredient ingredient );
        Task DeleteByIdAsync( int id );
    }
}