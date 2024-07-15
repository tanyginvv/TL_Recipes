namespace Recipes.Domain.Entities
{
    public class Ingredient
    {
        public int Id { get; init; }
        public int RecipeId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public Recipe Recipe { get; private set; }

        public Ingredient( string title, string description, int recipeId )
        {
            Title = title;
            Description = description;
            RecipeId = recipeId;
        }

        public void SetTitle( string title )
        {
            Title = title;
        }

        public void SetDescription( string description )
        {
            Description = description;
        }

        public void SetRecipe( Recipe recipe )
        {
            Recipe = recipe;
            RecipeId = recipe.Id;
        }
    }
}
