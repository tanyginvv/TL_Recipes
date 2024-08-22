namespace Recipes.Domain.Entities;

public class User : Entity
{
    public string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Login { get; set; }
    public string PasswordHash { get; set; }
    public IReadOnlyList<Recipe> Recipes { get; set; }
    public UserAuthToken AuthToken { get; set; }

    public User( string name, string login, string passwordHash )
    {
        Name = name;
        Login = login;
        PasswordHash = passwordHash;
        Recipes = new List<Recipe>();
    }
}