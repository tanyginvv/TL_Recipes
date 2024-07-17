using Recipes.Application.UseCases.Recipes.Dtos;

namespace Recipes.Application.UseCases.Recipes.Commands.CreateRecipe
{
    public class CreateRecipeCommand
    {
        public required string Name { get; init; }
        public required string Description { get; init; }
        public required int CookTime { get; init; }
        public required int CountPortion { get; init; }
        public required string ImageUrl { get; init; }
        public required List<TagDto> Tags { get; init; }
        public required List<StepDto> Steps { get; init; }
        public required List<IngredientDto> Ingredients { get; init; }
    }
}
