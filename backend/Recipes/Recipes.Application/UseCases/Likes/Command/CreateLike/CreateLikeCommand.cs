namespace Recipes.Application.UseCases.Likes.Command.CreateLike;

public class CreateLikeCommand
{
    public int UserId { get; init; }
    public int RecipeId { get; init; }
}