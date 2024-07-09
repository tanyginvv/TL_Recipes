namespace Recipes.Application.Ingredients.Commands.DeleteIngredient
{
    public class DeleteIngredientCommand
    {
        public int Id { get; init; }
        public int RecipeId { get; init; }
    }
}