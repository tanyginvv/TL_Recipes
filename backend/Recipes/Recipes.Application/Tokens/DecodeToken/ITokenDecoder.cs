using System.IdentityModel.Tokens.Jwt;

namespace Recipes.Application.Tokens.DecodeToken;

public interface ITokenDecoder
{
    JwtSecurityToken DecodeToken( string accessToken );
}