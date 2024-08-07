namespace Recipes.Domain.Entities
{
    public class Favourite : Entity
    {
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public Favourite( int recipeId, int userId )
        {
            RecipeId = recipeId;
            UserId = userId;
        }
    }
}