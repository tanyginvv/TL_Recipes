namespace Recipes.Application.UseCases.Steps.Commands.UpdateStep;

public class UpdateStepCommand
{
    public required int StepId { get; init; }
    public required int StepNumber { get; init; }
    public required string StepDescription { get; init; }
}