namespace Recipes.Application.Steps
{
    public class CreateStepCommand
    {
        public int StepNumber { get; init; }
        public string StepDescription { get; init; }
        public int RecipeId { get; init; }
    }
}