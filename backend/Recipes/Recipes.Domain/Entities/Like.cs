namespace Recipes.Domain.Entities;

public class Like : Entity
{
    public int RecipeId { get; init; }
    public Recipe Recipe { get; init; }
    public int UserId { get; init; }
    public User User { get; init; }

    public Like( int recipeId, int userId )
    {
        RecipeId = recipeId;
        UserId = userId;
    }
}