namespace Recipes.Application.UseCases.Likes.Command.DeleteLike;

public class DeleteLikeCommand
{
    public int UserId { get; init; }
    public int RecipeId { get; init; }
}