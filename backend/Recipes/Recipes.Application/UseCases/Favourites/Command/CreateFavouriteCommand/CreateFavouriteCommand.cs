namespace Recipes.Application.UseCases.Likes.Command;

public class CreateFavouriteCommand
{
    public int UserId { get; set; }
    public int RecipeId { get; set; }
}
