namespace Recipes.Application.UseCases.Recipes.Queries.SearchRecipe
{
    public class GetRecipesQuery
    {
        public List<string> SearchTerms { get; set; }
        public int PageNumber { get; set; } = 1;
    }
}