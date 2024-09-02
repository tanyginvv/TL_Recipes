namespace Recipes.Application.Options;

public class JwtOptions
{
    public int TokenValidityInMinutes { get; init; }
    public int RefreshTokenValidityInDays { get; init; }
    public string Secret { get; init; }
}