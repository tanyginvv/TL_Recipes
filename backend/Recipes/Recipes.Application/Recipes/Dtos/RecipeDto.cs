using Recipes.Domain.Entities;

namespace Recipes.Application.Recipes.Dtos
{
    public class RecipeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CookTime { get; set; }
        public int CountPortion { get; set; }
        public string ImageUrl { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Step> Steps { get; set; }
        public List<Ingredient> Ingredients { get; set; }
    }
}
