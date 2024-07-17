namespace Recipes.Domain.Entities
{
    public class Ingredient
    {
        public int Id { get; init; }
        public int RecipeId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Recipe Recipe { get; set; }

        public Ingredient( string title, string description, int recipeId )
        {
            Title = title;
            Description = description;
            RecipeId = recipeId;
        }
    }
}
