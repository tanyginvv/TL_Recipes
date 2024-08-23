using System.IdentityModel.Tokens.Jwt;

namespace Recipes.Infrastructure.Tokens.DecodeToken;

public static class TokenDecoder
{
    public static JwtSecurityToken DecodeToken( string accessToken )
    {
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        JwtSecurityToken token = handler.ReadToken( accessToken ) as JwtSecurityToken;
        return token;
    }
}