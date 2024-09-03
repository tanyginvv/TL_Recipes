namespace Recipes.Domain.Entities;

public class Favourite : Entity
{
    public int RecipeId { get; init; }
    public Recipe Recipe { get; init; }
    public int UserId { get; init; }
    public User User { get; init; }

    public Favourite( int recipeId, int userId )
    {
        RecipeId = recipeId;
        UserId = userId;
    }
}