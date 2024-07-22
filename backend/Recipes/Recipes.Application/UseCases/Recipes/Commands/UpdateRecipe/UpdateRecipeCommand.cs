using Recipes.Application.UseCases.Ingredients.Dtos;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Application.UseCases.Steps.Dtos;
using Recipes.Application.UseCases.Tags.Dtos;

namespace Recipes.Application.UseCases.Recipes.Commands.UpdateRecipe
{
    public class UpdateRecipeCommand
    {
        public required int Id { get; set; }
        public required string Name { get; init; }
        public required string Description { get; init; }
        public required int CookTime { get; init; }
        public required int PortionCount { get; init; }
        public required string ImageUrl { get; init; }
        public required List<TagDto> Tags { get; init; }
        public required List<StepDto> Steps { get; init; }
        public required List<IngredientDto> Ingredients { get; init; }
    }
}
