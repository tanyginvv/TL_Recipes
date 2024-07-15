namespace Recipes.Application.Ingredients.Commands.CreateIngredient
{
    public class CreateIngredientCommand
    {
        public required string Title { get; init; }
        public required string Description { get; init; }
        public required int RecipeId { get; init; }
    }
}
