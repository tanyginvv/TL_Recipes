namespace Recipes.Application.Steps.Commands.UpdateStepCommand
{
    public class UpdateStepCommand
    {
        public int RecipeId { get; init; }
        public int StepId { get; init; }
        public int StepNumber { get; init; }
        public string StepDescription { get; init; }
    }
}