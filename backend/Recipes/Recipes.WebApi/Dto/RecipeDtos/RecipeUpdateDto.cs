using System.ComponentModel.DataAnnotations;
using Recipes.WebApi.Dto.IngredientDtos;
using Recipes.WebApi.Dto.StepDtos;
using Recipes.WebApi.Dto.TagDtos;

namespace Recipes.WebApi.Dto.RecipeDtos;

public class RecipeUpdateDto
{
    [Required]
    public int UserId { get; set; }

    [Required]
    [StringLength( 50 )]
    public string Name { get; init; }

    [Required]
    [StringLength( 250 )]
    public string Description { get; init; }

    [Required]
    public int CookTime { get; init; }

    [Required]
    public int PortionCount { get; init; }

    [Required]
    public string ImageUrl { get; init; }

    [Required]
    public ICollection<IngredientApiDto> Ingredients { get; init; }

    [Required]
    public ICollection<StepApiDto> Steps { get; init; }

    [Required]
    public ICollection<TagApiDto> Tags { get; init; }
}