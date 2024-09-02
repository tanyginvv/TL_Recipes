using System.IdentityModel.Tokens.Jwt;
using Recipes.Application.Tokens.DecodeToken;

namespace Recipes.Infrastructure.TokenUtils.DecodeToken;

public class TokenDecoder : ITokenDecoder
{
    public JwtSecurityToken DecodeToken( string accessToken )
    {
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        JwtSecurityToken token = handler.ReadToken( accessToken ) as JwtSecurityToken;
        return token;
    }
}