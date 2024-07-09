namespace Recipes.Application.Steps.Dtos
{
    public class CreateStepCommandDto
    {
        public int StepNumber { get; init; }
        public string StepDescription { get; init; }
        public int RecipeId { get; init; }
    }
}
