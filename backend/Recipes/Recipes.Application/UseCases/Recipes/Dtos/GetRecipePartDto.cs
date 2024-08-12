﻿namespace Recipes.Application.UseCases.Recipes.Dtos;

public class GetRecipePartDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required int CookTime { get; init; }
    public required int PortionCount { get; init; }
    public required string ImageUrl { get; init; }
    public required ICollection<TagDto> Tags { get; init; }
}