using Microsoft.Extensions.Configuration;
using Recipes.Application.Tokens;

namespace Infrastructure.ConfigurationUtils.Token
{
    public class TokenConfiguration : ITokenConfiguration
    {
        private readonly IConfiguration _configuration;

        // Конструктор с параметром IConfiguration
        public TokenConfiguration( IConfiguration configuration )
        {
            _configuration = configuration;
        }
        public int GetAccessTokenValidityInMinutes()
        {
            string result = _configuration[ "JWTOptions:TokenValidityInMinutes" ];
            return int.Parse( result );
        }

        public int GetRefreshTokenValidityInDays()
        {
            string result = _configuration[ "JWTOptions:RefreshTokenValidityInDays" ];
            return int.Parse( result );
        }

        public string GetSecret()
        {
            string result = _configuration[ "JWTOptions:Secret" ];
            return result;
        }
    }
}