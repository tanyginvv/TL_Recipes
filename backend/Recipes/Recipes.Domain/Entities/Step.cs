namespace Recipes.Domain.Entities
{
    public class Step
    {
        public int Id { get; init; }
        public int RecipeId { get; init; }
        public int StepNumber { get; private set; }
        public string StepDescription { get; private set; }
        public Recipe Recipe { get; init; }

        public Step( int stepNumber, string stepDescription, int recipeId )
        {
            StepNumber = stepNumber;
            StepDescription = stepDescription;
            RecipeId = recipeId;
        }

        public void SetStepNumber( int stepNumber )
        {
            StepNumber = stepNumber;
        }

        public void SetStepDescription( string stepDescription )
        {
            StepDescription = stepDescription;
        }
    }
}