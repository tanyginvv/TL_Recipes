namespace Recipes.Domain.Entities
{
    public class User
    {
        public int Id { get; init; }
        public string Name { get; private set; }
        public string Login { get; private set; }
        public string PasswordHash { get; private set; }
        public string Description { get; private set; }

        public User( string login, string passwordHash )
        {
            Login = login;
            PasswordHash = passwordHash;
        }

        public void SetName( string name )
        {
            Name = name;
        }

        public void SetDescription( string description )
        {
            Description = description;
        }

        public void SetLogin( string login )
        {
            Login = login;
        }

        public void SetPasswordHash( string passwordHash )
        {
            PasswordHash = passwordHash;
        }
    }
}