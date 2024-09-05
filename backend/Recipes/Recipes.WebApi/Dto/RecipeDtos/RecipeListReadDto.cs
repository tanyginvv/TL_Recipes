using Recipes.Application.UseCases.Recipes.Dtos;

namespace Recipes.WebApi.Dto.RecipeDtos
{
    public class RecipeListReadDto
    {
        public IEnumerable<GetRecipePartDto> GetRecipePartDtos {  get; set; }
        public bool IsNextPageAvailable { get; set; }
    }
}