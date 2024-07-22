using Recipes.Application.UseCases.Ingredients.Dtos;
using Recipes.Application.UseCases.Steps.Dtos;
using Recipes.Application.UseCases.Tags.Dtos;

namespace Recipes.Application.UseCases.Recipes.Dtos
{
    public class RecipeDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required int CookTime { get; set; }
        public required int PortionCount { get; set; }
        public required string ImageUrl { get; set; }
        public required List<TagDtoUseCases> Tags { get; set; }
        public required List<StepDtoUseCases> Steps { get; set; }
        public required List<IngredientDtoUseCases> Ingredients { get; set; }
    }
}
