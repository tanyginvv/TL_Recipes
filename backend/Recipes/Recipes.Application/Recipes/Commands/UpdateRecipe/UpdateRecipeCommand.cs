namespace Recipes.Application.Recipes.Commands.UpdateRecipe
{
    public class UpdateRecipeCommand
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public int CookTime { get; init; }
        public int CountPortion { get; init; }
        public string ImageUrl { get; init; }
    }
}
