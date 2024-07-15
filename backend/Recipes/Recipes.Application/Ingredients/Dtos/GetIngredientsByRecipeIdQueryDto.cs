using Recipes.Domain.Entities;

namespace Recipes.Application.Ingredients.Dtos
{
    public class GetIngredientsByRecipeIdQueryDto
    {
        public int RecipeId { get; set; }
        public List<Ingredient> Ingredients { get; set; }
    }
}