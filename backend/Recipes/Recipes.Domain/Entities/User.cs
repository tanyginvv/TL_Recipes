namespace Recipes.Domain.Entities;

public class User : Entity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Login { get; set; }
    public string PasswordHash { get; set; }
    public IReadOnlyList<Recipe> Recipes { get; set; }
    public UserAuthorizationToken AuthorizationToken { get; set; }

    public User( string name, string login, string passwordHash, string description )
    {
        Name = name;
        Login = login;
        PasswordHash = passwordHash;
        Description = description;
        Recipes = new List<Recipe>();
    }
}