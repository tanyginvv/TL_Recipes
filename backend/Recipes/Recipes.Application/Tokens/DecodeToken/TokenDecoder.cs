using System.IdentityModel.Tokens.Jwt;

namespace Recipes.Application.Tokens.DecodeToken;

public class TokenDecoder
{
    public static JwtSecurityToken DecodeToken( string accessToken )
    {
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        JwtSecurityToken token = handler.ReadToken( accessToken ) as JwtSecurityToken;
        return token;
    }
}