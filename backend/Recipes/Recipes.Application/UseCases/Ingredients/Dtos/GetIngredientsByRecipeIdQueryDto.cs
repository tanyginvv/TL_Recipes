using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Ingredients.Dtos
{
    public class GetIngredientsByRecipeIdQueryDto
    {
        public required int RecipeId { get; set; }
        public required List<Ingredient> Ingredients { get; set; }
    }
}