using Recipes.Domain.Entities;

namespace Recipes.Application.Ingredients.Queries.GetIngredientsByRecipeIdQuery
{
    public class GetIngredientsByRecipeIdQuery
    {
        public int RecipeId { get; init; }
        public List<Ingredient> Ingredients { get; init; }
    }
}