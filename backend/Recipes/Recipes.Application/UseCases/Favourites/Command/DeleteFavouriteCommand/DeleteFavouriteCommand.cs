namespace Recipes.Application.UseCases.Likes.Command;

public class DeleteFavouriteCommand
{
    public int UserId { get; set; }
    public int RecipeId { get; set; }
}
