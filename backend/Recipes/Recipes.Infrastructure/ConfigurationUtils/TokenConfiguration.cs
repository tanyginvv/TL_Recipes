using Microsoft.Extensions.Options;
using Recipes.Application.Tokens;

namespace Recipes.Infrastructure.ConfigurationUtils;

public class TokenConfiguration(IOptions<JwtOptions> jwtOptions )  : ITokenConfiguration
{
    public int GetAccessTokenValidityInMinutes()
    {
        return jwtOptions.Value.TokenValidityInMinutes;
    }

    public int GetRefreshTokenValidityInDays()
    {
        return jwtOptions.Value.RefreshTokenValidityInDays;
    }

    public string GetSecret()
    {
        return jwtOptions.Value.Secret;
    }
}