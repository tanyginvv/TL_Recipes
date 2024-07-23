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
        public required List<TagDto> Tags { get; init; }
        public required List<StepDto> Steps { get; init; }
        public required List<IngredientDto> Ingredients { get; init; }

    }
}