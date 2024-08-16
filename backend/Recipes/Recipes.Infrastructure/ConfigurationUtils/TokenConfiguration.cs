using Microsoft.Extensions.Configuration;
using Recipes.Application.Tokens;

namespace Recipes.Infrastructure.ConfigurationUtils;

public class TokenConfiguration( IConfiguration configuration ) : ITokenConfiguration
{
    public int GetAccessTokenValidityInMinutes()
    {
        string result = configuration[ "JWTOptions:TokenValidityInMinutes" ];
        return int.Parse( result );
    }

    public int GetRefreshTokenValidityInDays()
    {
        string result = configuration[ "JWTOptions:RefreshTokenValidityInDays" ];
        return int.Parse( result );
    }

    public string GetSecret()
    {
        string result = configuration[ "JWTOptions:Secret" ];
        return result;
    }
}