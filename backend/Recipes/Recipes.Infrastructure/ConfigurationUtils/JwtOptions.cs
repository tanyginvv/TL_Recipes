namespace Recipes.Infrastructure.ConfigurationUtils;
public class JwtOptions
{
    public int TokenValidityInMinutes { get; set; }
    public int RefreshTokenValidityInDays { get; set; }
    public string Secret { get; set; }
}