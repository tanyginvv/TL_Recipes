namespace Recipes.Domain.Entities;

public class Step : Entity
{
    public int RecipeId { get; init; }
    public int StepNumber { get; set; }
    public string StepDescription { get; set; }
    public Recipe Recipe { get; init; }

    public Step( int stepNumber, string stepDescription, int recipeId )
    {
        StepNumber = stepNumber;
        StepDescription = stepDescription;
        RecipeId = recipeId;
    }
}