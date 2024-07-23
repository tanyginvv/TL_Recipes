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
        public required List<TagDto> Tags { get; set; }
        public required List<StepDto> Steps { get; set; }
        public required List<IngredientDto> Ingredients { get; set; }
    }
}
