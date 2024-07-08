namespace Recipes.Domain.Entities
{
    public class Step
    {
        public int Id { get; init; }
        public int RecipeId { get; init; }
        public int StepNumber { get; private set; }
        public string StepDescription { get; private set; }

        public Step( int stepNumber, string stepDescription )
        {
            StepNumber = stepNumber;
            StepDescription = stepDescription;
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