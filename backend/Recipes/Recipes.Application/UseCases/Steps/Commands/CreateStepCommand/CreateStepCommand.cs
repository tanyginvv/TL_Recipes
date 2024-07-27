namespace Recipes.Application.UseCases.Steps.Commands.CreateStepCommand
{
    public class CreateStepCommand
    {
        public required int StepNumber { get; init; }
        public required string StepDescription { get; init; }
        public required int RecipeId { get; init; }
    }
}