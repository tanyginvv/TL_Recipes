using Recipes.Application.Ingredients.Dtos;
using Recipes.Application.Steps.Dtos;
using Recipes.Application.Tags.Dtos;

namespace Recipes.Application.Recipes.Dtos
{
    public class RecipeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CookTime { get; set; }
        public int CountPortion { get; set; }
        public string ImageUrl { get; set; }
        public List<TagDto> Tags { get; set; }
        public List<StepDto> Steps { get; set; }
        public List<IngredientDto> Ingredients { get; set; }
    }
}
