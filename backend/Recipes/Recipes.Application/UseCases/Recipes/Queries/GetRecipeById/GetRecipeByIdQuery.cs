namespace Recipes.Application.UseCases.Recipes.Queries.GetRecipeById;

public class GetRecipeByIdQuery
{
    public required int Id { get; init; }
    public required int UserId { get; init; }
}