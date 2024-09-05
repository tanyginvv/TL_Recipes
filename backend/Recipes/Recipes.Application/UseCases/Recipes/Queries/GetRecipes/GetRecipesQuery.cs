namespace Recipes.Application.UseCases.Recipes.Queries.GetRecipes;

public class GetRecipesQuery
{
    public int UserId { get; set; }
    public List<string> SearchTerms { get; init; }
    public int PageNumber { get; init; } = 1;
    public RecipeQueryType RecipeQueryType { get; init; }
}