using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Recipes.Application.Options;
using Recipes.Application.Tokens;
using Recipes.Application.Tokens.CreateToken;

namespace Recipes.Infrastructure.TokenUtils.CreateToken;

public class TokenCreator( IOptions<JwtOptions> jwtOptions ) : ITokenCreator
{
    public string GenerateAccessToken( int userId )
    {
        List<Claim> claims = new List<Claim>()
        {
            new Claim( nameof(userId), userId.ToString())
        };

        SigningCredentials signingCredentials = new SigningCredentials(
           new SymmetricSecurityKey( Encoding.UTF8.GetBytes( jwtOptions.Value.Secret ) ), SecurityAlgorithms.HmacSha256 );

        JwtSecurityToken token = new JwtSecurityToken(
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddMinutes( jwtOptions.Value.TokenValidityInMinutes ),
            claims: claims
            );

        string tokenString = new JwtSecurityTokenHandler().WriteToken( token );

        return tokenString;
    }

    public string GenerateRefreshToken()
    {
        return Guid.NewGuid().ToString();
    }
}