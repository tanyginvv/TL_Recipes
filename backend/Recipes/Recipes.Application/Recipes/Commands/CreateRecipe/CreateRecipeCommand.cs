using Recipes.Domain.Entities;

namespace Recipes.Application.Recipes.Commands.CreateRecipe
{
    public class CreateRecipeCommand
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public int CookTime { get; init; }
        public int CountPortion { get; init; }
        public string ImageUrl { get; init; }
        public List<string> Tags { get; init; }
        public List<CreateStepDto> Steps { get; init; }
        public List<CreateIngredientDto> Ingredients { get; init; }
    }
}
