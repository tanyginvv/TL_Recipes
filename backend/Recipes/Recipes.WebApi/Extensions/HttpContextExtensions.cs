using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Recipes.Application.Tokens.DecodeToken;

namespace Recipes.WebApi.Extensions
{
    public static class HttpContextExtensions
    {
        private static readonly ITokenDecoder tokenDecoder;
        public static int GetUserIdFromAccessToken( this HttpContext httpContext )
        {
            string accessToken = httpContext.Request.Headers[ "Access-Token" ];

            JwtSecurityToken token = tokenDecoder.DecodeToken( accessToken );

            Claim userIdClaim = token.Claims.FirstOrDefault( claim => claim.Type == "userId" );

            _ = int.TryParse( userIdClaim?.Value, out int userId );

            return userId;
        }
    }
}