using Recipes.Application.Ingredients.Dtos;
using Recipes.Application.Steps.Dtos;
using Recipes.Application.Tags.Dtos;
using Recipes.Domain.Entities;

namespace Recipes.Application.Recipes.Commands
{
    public class CreateRecipeCommand
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public int CookTime { get; init; }
        public int CountPortion { get; init; }
        public string ImageUrl { get; init; }
        public IReadOnlyList<TagDto> Tags { get; init; }
        public IReadOnlyList<StepDto> Steps { get; init; }
        public IReadOnlyList<IngredientDto> Ingredients { get; init; }
    }
}
