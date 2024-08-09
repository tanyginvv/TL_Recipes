using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Steps.Commands;

public class CreateStepCommand
{
    public required int StepNumber { get; init; }
    public required string StepDescription { get; init; }
    public required Recipe Recipe { get; set; }
}