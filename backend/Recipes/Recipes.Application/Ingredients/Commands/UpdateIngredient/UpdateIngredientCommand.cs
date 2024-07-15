namespace Recipes.Application.Ingredients.Commands.UpdateIngredient
{
    public class UpdateIngredientCommand
    {
        public required int Id { get; init; }
        public required string Title { get; init; }
        public required string Description { get; init; }
    }
}
