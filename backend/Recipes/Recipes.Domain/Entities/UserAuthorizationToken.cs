namespace Recipes.Domain.Entities
{
    public class UserAuthorizationToken
    {
        public long UserId { get; init; }
        public string RefreshToken { get; init; }
        public DateTime ExpiryDate { get; init; }

        public UserAuthorizationToken( long userId, string refreshToken, DateTime expiryDate )
        {
            UserId = userId;
            RefreshToken = refreshToken;
            ExpiryDate = expiryDate;
        }
    }
}