namespace Recipes.API.Dto.RecipeDtos
{
    public class RecipeCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CookTime { get; set; }
        public int CountPortion { get; set; }
        public string ImageUrl { get; set; }
    }
}