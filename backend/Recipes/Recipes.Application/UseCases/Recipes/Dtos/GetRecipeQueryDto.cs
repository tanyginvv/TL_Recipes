namespace Recipes.Application.UseCases.Recipes.Dtos;

public class GetRecipeQueryDto
{
    public required int Id { get; init; }
    public required string AuthorLogin { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required int CookTime { get; init; }
    public required int PortionCount { get; init; }
    public required string ImageUrl { get; init; }
    public required ICollection<TagDto> Tags { get; init; }
    public required ICollection<StepDto> Steps { get; init; }
    public required ICollection<IngredientDto> Ingredients { get; init; }
}