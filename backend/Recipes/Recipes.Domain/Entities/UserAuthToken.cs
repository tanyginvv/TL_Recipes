namespace Recipes.Domain.Entities;

public class UserAuthToken
{
    public int UserId { get; init; }
    public User User { get; init; }
    public string RefreshToken { get; init; }
    public DateTime ExpiryDate { get; init; }

    public UserAuthToken( int userId, string refreshToken, DateTime expiryDate )
    {
        UserId = userId;
        RefreshToken = refreshToken;
        ExpiryDate = expiryDate;
    }
}