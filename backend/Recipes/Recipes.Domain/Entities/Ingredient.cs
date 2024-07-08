namespace Recipes.Domain.Entities
{
    public class Ingredient
    {
        public int Id { get; init; }
        public int RecipeId { get; init; }
        public string Title { get; private set; }
        public string Description { get; private set; }

        public Ingredient( string title, string description )
        {
            Title = title;
            Description = description;
        }

        public void SetTitle( string title )
        {
            Title = title;
        }

        public void SetDescription( string description )
        {
            Description = description;
        }
    }
}