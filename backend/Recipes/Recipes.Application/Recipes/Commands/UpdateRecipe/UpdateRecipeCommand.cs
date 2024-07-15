using Recipes.Application.Ingredients.Dtos;
using Recipes.Application.Steps.Dtos;
using Recipes.Application.Tags.Dtos;

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
        public IReadOnlyList<TagDto> Tags { get; init; }
        public IReadOnlyList<StepDto> Steps { get; init; }
        public IReadOnlyList<IngredientDto> Ingredients { get; init; }
    }
}
