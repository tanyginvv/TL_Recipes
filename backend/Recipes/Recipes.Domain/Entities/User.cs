namespace Recipes.Domain.Entities
{
    public class User
    {
        public int Id { get; init; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string Description { get; set; }

        public User( string login, string passwordHash )
        {
            Login = login;
            PasswordHash = passwordHash;
        }
    }
}