namespace Recipes.Application.Ingredients.Commands.CreateIngredient
{
    public class CreateIngredientCommand
    {
        public string Title { get; init; }
        public string Description { get; init; }
        public int RecipeId { get; init; }
    }
}
