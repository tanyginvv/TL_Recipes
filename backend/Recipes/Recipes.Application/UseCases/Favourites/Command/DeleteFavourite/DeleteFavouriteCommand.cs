namespace Recipes.Application.UseCases.Favourites.Command.DeleteFavourite;

public class DeleteFavouriteCommand
{
    public int UserId { get; init; }
    public int RecipeId { get; init; }
}