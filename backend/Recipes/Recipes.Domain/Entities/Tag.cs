namespace Recipes.Domain.Entities;

public class Tag : Entity
{
    public string Name { get; init; }
    public ICollection<Recipe> Recipes { get; set; }

    public Tag( string name )
    {
        Name = name;
    }
}