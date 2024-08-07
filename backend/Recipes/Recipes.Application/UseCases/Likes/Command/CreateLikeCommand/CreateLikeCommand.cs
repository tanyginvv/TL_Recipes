namespace Recipes.Application.UseCases.Likes.Command
{
    public class CreateLikeCommand
    {
        public int UserId { get; set; }
        public int RecipeId { get; set; }
    }
}
