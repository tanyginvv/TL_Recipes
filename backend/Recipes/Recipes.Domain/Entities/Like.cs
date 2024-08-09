namespace Recipes.Domain.Entities;

public class Like : Entity
{
    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }

    public Like( int recipeId, int userId )
    {
        RecipeId = recipeId;
        UserId = userId;
    }
}
