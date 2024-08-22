using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using Recipes.Infrastructure.Tokens.DecodeToken;
using Recipes.Infrastructure.Tokens.VerificationToken;

namespace Recipes.Infrastructure.JwtAuthorization;

public class JwtAuthorizationAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization( AuthorizationFilterContext context )
    {
        ITokenConfiguration configuration = context.HttpContext.RequestServices.GetService<ITokenConfiguration>();

        string accessToken = context.HttpContext.Request.Headers[ "Access-Token" ];
        if ( string.IsNullOrEmpty( accessToken ) )
        {
            context.Result = new ForbidResult();
            return;
        }

        TokenSignatureVerificator tokenSignatureVerificator = new TokenSignatureVerificator();

        tokenSignatureVerificator.VerifySignature( accessToken, configuration.GetSecret() );
        if ( !tokenSignatureVerificator.TokenIsValid )
        {
            context.Result = new ForbidResult();
            return;
        }

        TokenDecoder tokenDecoder = new TokenDecoder();
        JwtSecurityToken token = tokenDecoder.DecodeToken( accessToken );
        DateTime expDate = DateTimeOffset.FromUnixTimeSeconds( token.Payload.Exp.Value ).UtcDateTime;
        if ( DateTime.UtcNow > expDate )
        {
            context.Result = new ForbidResult();
            return;
        }

        Claim userIdClaim = token.Claims.FirstOrDefault( claim => claim.Type == "userId" );
        if ( userIdClaim is null )
        {
            context.Result = new ForbidResult();
            return;
        }

        if ( !int.TryParse( userIdClaim.Value, out int userId ) )
        {
            context.Result = new ForbidResult();
            return;
        }
    }
}