using Recipes.Application.UseCases.Recipes.Dtos;

namespace Recipes.Application.UseCases.Recipes.Commands.CreateRecipe
{
    public class CreateRecipeCommand
    {
        public required string Name { get; init; }
        public required string Description { get; init; }
        public required int CookTime { get; init; }
        public required int PortionCount { get; init; }
        public required string ImageUrl { get; init; }
        public required ICollection<TagDto> Tags { get; init; }
        public required ICollection<StepDto> Steps { get; init; }
        public required ICollection<IngredientDto> Ingredients { get; init; }
    }
}
