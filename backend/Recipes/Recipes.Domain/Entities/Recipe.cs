namespace Recipes.Domain.Entities
{
    public class Recipe
    {
        public int Id { get; init; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CookTime { get; set; }
        public int PortionCount { get; set; }
        public string ImageUrl { get; set; }

        public List<Tag> Tags { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public List<Step> Steps { get; set; }

        public Recipe( string name, string description, int cookTime, int portionCount, string imageUrl )
        {
            Name = name;
            Description = description;
            CookTime = cookTime;
            PortionCount = portionCount;
            ImageUrl = imageUrl;
            Tags = new List<Tag>();
            Ingredients = new List<Ingredient>();
            Steps = new List<Step>();
        }
    }
}
