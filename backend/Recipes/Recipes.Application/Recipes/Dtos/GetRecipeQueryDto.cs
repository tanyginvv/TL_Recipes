using Recipes.Application.Ingredients.Dtos;
using Recipes.Application.Steps.Dtos;
using Recipes.Application.Tags.Dtos;

namespace Recipes.Application.Recipes.Dtos
{
    public class GetRecipeByIdQueryDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public int CookTime { get; init; }
        public int CountPortion { get; init; }
        public string ImageUrl { get; init; }
        public List<TagDto> Tags { get; init; }
        public List<StepDto> Steps { get; init; }
        public List<IngredientDto> Ingredients { get; init; }

    }
}