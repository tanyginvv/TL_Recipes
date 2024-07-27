using Recipes.Application.UseCases.Ingredients.Dtos;
using Recipes.Application.UseCases.Steps.Dtos;
using Recipes.Application.UseCases.Tags.Dtos;

namespace Recipes.Application.UseCases.Recipes.Dtos
{
    public class GetRecipeByIdQueryDto
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
        public required string Description { get; init; }
        public required int CookTime { get; init; }
        public required int PortionCount { get; init; }
        public required string ImageUrl { get; init; }
        public required List<TagDtoUseCases> Tags { get; init; }
        public required List<StepDtoUseCases> Steps { get; init; }
        public required List<IngredientDtoUseCases> Ingredients { get; init; }

    }
}