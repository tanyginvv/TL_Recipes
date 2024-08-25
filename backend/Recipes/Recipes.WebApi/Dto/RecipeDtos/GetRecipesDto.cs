namespace Recipes.WebApi.Dto.RecipeDtos
{
    public class GetRecipesDto
    {
        public int PageNumber { get; init; } = 1;
        public List<string> SearchTerms { get; init; } = null;
        public bool IsUser { get; init; } = false;
    }
}