using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Recipes.Application.Tokens;
using Recipes.Application.Tokens.CreateToken;

namespace Recipes.Infrastructure.Tokens.CreateToken;

public class TokenCreator( ITokenConfiguration tokenConfiguration ) : ITokenCreator
{
    public string GenerateAccessToken( int userId )
    {
        List<Claim> claims = new List<Claim>()
        {
            new Claim( nameof(userId), userId.ToString())
        };

        SigningCredentials signingCredentials = new SigningCredentials(
           new SymmetricSecurityKey( Encoding.UTF8.GetBytes( tokenConfiguration.GetSecret() ) ), SecurityAlgorithms.HmacSha256 );

        JwtSecurityToken token = new JwtSecurityToken(
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddMinutes( tokenConfiguration.GetAccessTokenValidityInMinutes() ),
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