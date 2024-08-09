using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Steps.Dtos;

public class GetStepsByRecipeIdQueryDto
{
    public required int RecipeId { get; init; }
    public required IReadOnlyList<Step> Steps { get; set; }
}