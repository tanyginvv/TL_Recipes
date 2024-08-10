namespace Recipes.Application.UseCases.Recipes.Queries.GetRecipes;

public class GetRecipesQuery
{
    public int UserId { get; set; }
    public List<string> SearchTerms { get; set; }
    public int PageNumber { get; set; } = 1;
}