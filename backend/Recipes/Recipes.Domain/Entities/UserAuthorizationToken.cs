namespace Recipes.Domain.Entities;

public class UserAuthorizationToken
{
    public int UserId { get; init; }
    public User User { get; init; }
    public string RefreshToken { get; init; }
    public DateTime ExpiryDate { get; init; }

    public UserAuthorizationToken( int userId, string refreshToken, DateTime expiryDate )
    {
        UserId = userId;
        RefreshToken = refreshToken;
        ExpiryDate = expiryDate;
    }
}