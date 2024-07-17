using Recipes.Application.Ingredients.Dtos;
using Recipes.Application.Steps.Dtos;
using Recipes.Application.Tags.Dtos;

public class RecipeCreateDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int CookTime { get; set; }
    public int CountPortion { get; set; }
    public IFormFile Image { get; set; }
    public required IReadOnlyList<TagDto> Tags { get; set; }
    public required IReadOnlyList<StepDto> Steps { get; set; }
    public required IReadOnlyList<IngredientDto> Ingredients { get; set; }
}
