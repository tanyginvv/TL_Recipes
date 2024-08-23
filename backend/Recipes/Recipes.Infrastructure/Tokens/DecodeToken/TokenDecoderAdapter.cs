using System.IdentityModel.Tokens.Jwt;
using Recipes.Application.Tokens.DecodeToken;

namespace Recipes.Infrastructure.Tokens.DecodeToken;

public class TokenDecoderAdapter : ITokenDecoder
{
    public JwtSecurityToken DecodeToken( string accessToken )
    {
        return TokenDecoder.DecodeToken( accessToken );
    }
}