namespace Recipes.Application.Ingredients.Commands.UpdateIngredient
{
    public class UpdateIngredientCommand
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
    }
}
