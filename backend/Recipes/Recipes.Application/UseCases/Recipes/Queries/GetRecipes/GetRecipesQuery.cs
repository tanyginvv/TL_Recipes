namespace Recipes.Application.UseCases.Recipes.Queries.GetRecipes;

public class GetRecipesQuery
{
    public int UserId { get; set; }
    public List<string> SearchTerms { get; init; }
    public bool IsAuth { get; init; }
    public int PageNumber { get; init; } = 1;
    public bool isFavourite { get; init; } = false;
}