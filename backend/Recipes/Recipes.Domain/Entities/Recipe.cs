
namespace Recipes.Domain.Entities;

public class Recipe : Entity
{
    public int AuthorId { get; init; }
    public User Author { get; init; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int CookTime { get; set; }
    public int PortionCount { get; set; }
    public string ImageUrl { get; set; }

    public ICollection<Like> Likes { get; set; }
    public ICollection<Favourite> Favourites { get; set; }

    public ICollection<Tag> Tags { get; set; }
    public ICollection<Ingredient> Ingredients { get; set; }
    public ICollection<Step> Steps { get; set; }

    public Recipe( int authorId, string name, string description, int cookTime, int portionCount, string imageUrl )
    {
        AuthorId = authorId;
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