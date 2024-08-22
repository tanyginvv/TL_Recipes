using System.ComponentModel.DataAnnotations;
using Recipes.WebApi.Dto.TagDtos;

namespace Recipes.WebApi.Dto.RecipeDtos;

public class RecipePartReadDto
{
    [Required]
    public required int Id { get; init; }

    [Required]
    public required string AuthorLogin { get; init; }

    [Required]
    public required string Name { get; init; }

    [Required]
    public required string Description { get; init; }

    [Required]
    public required int CookTime { get; init; }

    [Required]
    public required int PortionCount { get; init; }

    [Required]
    public required string ImageUrl { get; init; }

    [Required]
    public required ICollection<TagApiDto> Tags { get; init; }
}