namespace Recipes.Domain.Entities
{
    public class Recipe
    {
        public int Id { get; init; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int CookTime { get; private set; }
        public int CountPortion { get; private set; }
        public string ImageUrl { get; private set; }

        public ICollection<Tag> Tags { get; private set; }
        public ICollection<Ingredient> Ingredients { get; private set; }
        public ICollection<Step> Steps { get; private set; }

        public Recipe( string name, string description, int cookTime, int countPortion, string imageUrl )
        {
            Name = name;
            Description = description;
            CookTime = cookTime;
            CountPortion = countPortion;
            ImageUrl = imageUrl;
            Tags = new List<Tag>();
            Ingredients = new List<Ingredient>();
            Steps = new List<Step>();
        }

        public void SetName( string name )
        {
            Name = name;
        }

        public void SetDescription( string description )
        {
            Description = description;
        }

        public void SetCookTime( int cookTime )
        {
            CookTime = cookTime;
        }

        public void SetCountPortion( int countPortion )
        {
            CountPortion = countPortion;
        }

        public void SetImageUrl( string imageUrl )
        {
            ImageUrl = imageUrl;
        }

        public void AddTag( Tag tag )
        {
            Tags.Add( tag );
        }

        public void RemoveTag( Tag tag )
        {
            Tags.Remove( tag );
        }

        public void SetTags( ICollection<Tag> tags )
        {
            Tags = new List<Tag>( tags );
        }

        public void AddIngredient( Ingredient ingredient )
        {
            Ingredients.Add( ingredient );
        }

        public void RemoveIngredient( Ingredient ingredient )
        {
            Ingredients.Remove( ingredient );
        }

        public void SetIngredients( ICollection<Ingredient> ingredients )
        {
            Ingredients = new List<Ingredient>( ingredients );
        }

        public void AddStep( Step step )
        {
            Steps.Add( step );
        }

        public void RemoveStep( Step step )
        {
            Steps.Remove( step );
        }

        public void SetSteps( ICollection<Step> steps )
        {
            Steps = new List<Step>( steps );
        }
    }
}
