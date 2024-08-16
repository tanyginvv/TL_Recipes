using Recipes.Application.PasswordHasher;

namespace Recipes.Infrastructure;

public class PasswordHasher : IPasswordHasher
{
    public string GeneratePassword( string password )
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword( password );
    }

    public bool VerifyPassword( string password, string hashedPassword )
    {
        return BCrypt.Net.BCrypt.EnhancedVerify( password, hashedPassword );
    }
}