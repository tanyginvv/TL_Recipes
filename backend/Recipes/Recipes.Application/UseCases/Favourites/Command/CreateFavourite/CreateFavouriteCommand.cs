namespace Recipes.Application.UseCases.Favourites.Command.CreateFavourite;

public class CreateFavouriteCommand
{
    public int UserId { get; init; }
    public int RecipeId { get; init; }
}