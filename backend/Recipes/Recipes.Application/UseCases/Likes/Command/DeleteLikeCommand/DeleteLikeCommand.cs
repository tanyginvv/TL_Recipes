namespace Recipes.Application.UseCases.Likes.Command
{
    public class DeleteLikeCommand
    {
        public int UserId { get; set; }
        public int RecipeId { get; set; }
    }
}
