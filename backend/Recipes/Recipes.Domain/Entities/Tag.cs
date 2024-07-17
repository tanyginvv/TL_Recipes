namespace Recipes.Domain.Entities
{
    public class Tag
    {
        public int Id { get; init; }
        public string Name { get; set; }
        public ICollection<Recipe> Recipes { get; set; }

        public Tag( string name )
        {
            Name = name;
        }
    }
}