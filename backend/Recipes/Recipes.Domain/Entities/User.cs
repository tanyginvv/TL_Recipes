namespace Recipes.Domain.Entities;

public class User : Entity
{
    public string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Login { get; set; }
    public string PasswordHash { get; set; }
    public ICollection<Recipe> Recipes { get; private set; }
    public ICollection<Like> Likes { get; set; }
    public ICollection<Favourite> Favourites { get; set; }
    public UserAuthToken AuthToken { get; private set; }

    public User( string name, string login, string passwordHash )
    {
        Name = name;
        Login = login;
        PasswordHash = passwordHash;
        Recipes = new List<Recipe>();
    }

    public User()
    {
    }
}