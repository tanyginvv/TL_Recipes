using Recipes.Domain.Entities;

namespace Recipes.Infrastructure.Repositories
{
    public interface IIngredientRepository
    {
        Task<IEnumerable<Ingredient>> GetAllIngredientsAsync();
        Task<IEnumerable<Ingredient>> GetIngredientByIdAsync( int id );
        Task<IEnumerable<Ingredient>> GetByRecipeIdAsync( int recipeId );
        Task AddIngredientAsync( Ingredient ingredient );
        Task UpdateIngredientAsync( Ingredient ingredient );
        Task DeleteIngredientAsync( int id );
    }
}