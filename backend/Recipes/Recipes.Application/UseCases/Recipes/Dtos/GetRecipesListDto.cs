namespace Recipes.Application.UseCases.Recipes.Dtos;

public class GetRecipesListDto
{
    public IEnumerable<GetRecipePartDto> GetRecipePartDtos { get; set; }
    public bool IsNextPageAvailable { get; set; }
}