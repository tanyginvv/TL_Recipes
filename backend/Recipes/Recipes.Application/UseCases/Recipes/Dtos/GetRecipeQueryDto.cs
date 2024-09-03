namespace Recipes.Application.UseCases.Recipes.Dtos;

public class GetRecipeQueryDto
{
    public required int Id { get; init; }
    public required int AuthorId { get; init; }
    public required string AuthorLogin { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required int CookTime { get; init; }
    public required int PortionCount { get; init; }
    public required int LikeCount { get; set; }
    public required bool IsLike { get; set; }
    public required int FavouriteCount { get; set; }
    public required bool IsFavourite { get; set; }
    public required string ImageUrl { get; init; }
    public required ICollection<TagDto> Tags { get; init; }
    public required ICollection<StepDto> Steps { get; init; }
    public required ICollection<IngredientDto> Ingredients { get; init; }
}