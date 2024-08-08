using System.ComponentModel.DataAnnotations;

namespace Recipes.WebApi.Dto.RecipeDtos;

public class RecipeReadIdDto
{
    [Required]
    public int Id { get; init; }
}