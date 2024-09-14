using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using Recipes.Application.Tokens.DecodeToken;
using Recipes.Application.Tokens.VerificationToken;
using Microsoft.Extensions.Options;
using Recipes.Application.Options;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Recipes.WebApi.JwtAuthorization;

public class JwtAuthorizationAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization( AuthorizationFilterContext context )
    {
        ILogger logger = Log.ForContext<JwtAuthorizationAttribute>();
        ITokenSignatureVerificator tokenSignatureVerificator = context.HttpContext.RequestServices.GetService<ITokenSignatureVerificator>();
        ITokenDecoder tokenDecoder = context.HttpContext.RequestServices.GetService<ITokenDecoder>();
        IOptions<JwtOptions> configuration = context.HttpContext.RequestServices.GetService<IOptions<JwtOptions>>();

        try
        {
            string accessToken = context.HttpContext.Request.Headers[ "Access-Token" ];
            if ( string.IsNullOrEmpty( accessToken ) )
            {
                logger.Warning( "Отсутствует Access-Token." );
                context.Result = new UnauthorizedResult();
                return;
            }

            if ( !tokenSignatureVerificator.VerifySignature( accessToken, configuration.Value.Secret ) )
            {
                logger.Warning( "Неверная подпись токена." );
                context.Result = new UnauthorizedResult();
                return;
            }

            JwtSecurityToken token = tokenDecoder.DecodeToken( accessToken );

            if ( token?.Payload?.Expiration is null || DateTime.UtcNow > DateTimeOffset.FromUnixTimeSeconds( token.Payload.Expiration.Value ).UtcDateTime )
            {
                logger.Warning( "Срок действия токена истек." );
                context.Result = new UnauthorizedResult();
                return;
            }

            Claim userIdClaim = token.Claims.FirstOrDefault( claim => claim.Type == "userId" );
            if ( userIdClaim is null || !int.TryParse( userIdClaim.Value, out int userId ) )
            {
                logger.Warning( "Отсутствует или некорректен userId в токене." );
                context.Result = new UnauthorizedResult();
                return;
            }

            context.HttpContext.Items[ "userId" ] = userId;
        }
        catch ( Exception ex )
        {
            logger.Error( ex, "Произошла ошибка при валидации токена." );
            context.Result = new UnauthorizedResult();
        }
    }
}