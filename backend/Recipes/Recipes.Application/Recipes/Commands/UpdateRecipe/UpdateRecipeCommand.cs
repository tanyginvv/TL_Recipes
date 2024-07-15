using Recipes.Application.Ingredients.Dtos;
using Recipes.Application.Steps.Dtos;
using Recipes.Application.Tags.Dtos;

namespace Recipes.Application.Recipes.Commands.UpdateRecipe
{
    public class UpdateRecipeCommand
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
        public required string Description { get; init; }
        public required int CookTime { get; init; }
        public required int CountPortion { get; init; }
        public required string ImageUrl { get; init; }
        public required IReadOnlyList<TagDto> Tags { get; init; }
        public required IReadOnlyList<StepDto> Steps { get; init; }
        public required IReadOnlyList<IngredientDto> Ingredients { get; init; }
    }
}
