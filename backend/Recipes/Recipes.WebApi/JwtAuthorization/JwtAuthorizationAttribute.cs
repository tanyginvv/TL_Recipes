using Recipes.Application.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using Recipes.Application.Tokens.DecodeToken;
using Recipes.Application.Tokens.VerificationToken;

namespace Recipes.WebApi.JwtAuthorization
{
    public class JwtAuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization( AuthorizationFilterContext context )
        {
            ILogger logger = context.HttpContext.RequestServices.GetService<ILogger<JwtAuthorizationAttribute>>();
            ITokenSignatureVerificator tokenSignatureVerificator = context.HttpContext.RequestServices.GetService<ITokenSignatureVerificator>();
            ITokenDecoder tokenDecoder = context.HttpContext.RequestServices.GetService<ITokenDecoder>();

            try
            {
                ITokenConfiguration configuration = context.HttpContext.RequestServices.GetService<ITokenConfiguration>();

                string accessToken = context.HttpContext.Request.Headers[ "Access-Token" ];
                if ( string.IsNullOrEmpty( accessToken ) )
                {
                    logger.LogWarning( "Отсутствует Access-Token." );
                    context.Result = new UnauthorizedResult();
                    return;
                }

                if ( !tokenSignatureVerificator.VerifySignature( accessToken, configuration.GetSecret() ) )
                {
                    logger.LogWarning( "Неверная подпись токена." );
                    context.Result = new UnauthorizedResult();
                    return;
                }

                JwtSecurityToken token = tokenDecoder.DecodeToken( accessToken );

                if ( token?.Payload?.Expiration is null || DateTime.UtcNow > DateTimeOffset.FromUnixTimeSeconds( token.Payload.Expiration.Value ).UtcDateTime )
                {
                    logger.LogWarning( "Срок действия токена истек." );
                    context.Result = new UnauthorizedResult();
                    return;
                }

                Claim userIdClaim = token.Claims.FirstOrDefault( claim => claim.Type == "userId" );
                if ( userIdClaim is null || !int.TryParse( userIdClaim.Value, out int userId ) )
                {
                    logger.LogWarning( "Отсутствует или некорректен userId в токене." );
                    context.Result = new UnauthorizedResult();
                    return;
                }

                context.HttpContext.Items[ "userId" ] = userId;
            }
            catch ( Exception ex )
            {
                logger.LogError( ex, "Произошла ошибка при валидации токена." );
                context.Result = new UnauthorizedResult();
            }
        }
    }
}