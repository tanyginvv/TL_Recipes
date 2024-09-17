namespace Recipes.Domain.Entities;

public class Ingredient : Entity
{
    public int RecipeId { get; init; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Recipe Recipe { get; init; }

    public Ingredient( string title, string description, int recipeId )
    {
        Title = title;
        Description = description;
        RecipeId = recipeId;
    }
}