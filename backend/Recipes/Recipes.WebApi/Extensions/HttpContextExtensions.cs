using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Recipes.Application.Tokens.DecodeToken;

namespace Recipes.WebApi.Extensions
{
    public static class HttpContextExtensions
    {
        public static int? GetUserIdFromAccessToken( this HttpContext httpContext, ITokenDecoder tokenDecoder )
        {
            string accessToken = httpContext.Request.Headers[ "Access-Token" ];
            if ( string.IsNullOrEmpty( accessToken ) )
            {
                return null;
            }

            JwtSecurityToken token = tokenDecoder.DecodeToken( accessToken );
            Claim userIdClaim = token.Claims.FirstOrDefault( claim => claim.Type == "userId" );

            if ( userIdClaim is not null && int.TryParse( userIdClaim.Value, out int userId ) )
            {
                return userId;
            }

            return null;
        }
    }
}