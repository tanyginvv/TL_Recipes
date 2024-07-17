using Recipes.Application.Ingredients.Dtos;
using Recipes.Application.Steps.Dtos;
using Recipes.Application.Tags.Dtos;

namespace Recipes.API.Dto.RecipeDtos
{
    public class RecipeReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CookTime { get; set; }
        public int CountPortion { get; set; }
        public string ImageUrl { get; set; }
        public IReadOnlyList<TagDto> Tags { get; set; }
        public IReadOnlyList<StepDto> Steps { get; set; }
        public IReadOnlyList<IngredientDto> Ingredients { get; set; }
    }
}