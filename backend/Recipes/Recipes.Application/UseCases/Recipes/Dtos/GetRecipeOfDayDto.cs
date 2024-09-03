namespace Recipes.Application.UseCases.Recipes.Dtos;

public class GetRecipeOfDayDto
{
    public required int Id { get; init; }
    public required string AuthorLogin { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required int CookTime { get; init; }
    public required int LikeCount { get; init; }
    public required int FavouriteCount { get; init; }
    public required string ImageUrl { get; init; }
}