using Recipes.Application.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using Recipes.Application.Tokens.DecodeToken;
using Recipes.Application.Tokens.VerificationToken;

namespace Recipes.WebApi.JwtAuthorization;

public class JwtAuthorizationAttribute : Attribute, IAuthorizationFilter
{
    private readonly ITokenDecoder tokenDecoder;
    private readonly ITokenSignatureVerificator tokenSignatureVerificator;

    public void OnAuthorization( AuthorizationFilterContext context )
    {
        ITokenConfiguration configuration = context.HttpContext.RequestServices.GetService<ITokenConfiguration>();

        string accessToken = context.HttpContext.Request.Headers[ "Access-Token" ];
        if ( string.IsNullOrEmpty( accessToken ) )
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if ( !tokenSignatureVerificator.VerifySignature( accessToken, configuration.GetSecret() ) )
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        JwtSecurityToken token;
        try
        {
            token = tokenDecoder.DecodeToken( accessToken );
        }
        catch
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if ( token is null || !token.Payload.Exp.HasValue || DateTime.UtcNow > DateTimeOffset.FromUnixTimeSeconds( token.Payload.Exp.Value ).UtcDateTime )
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        Claim userIdClaim = token.Claims.FirstOrDefault( claim => claim.Type == "userId" );
        if ( userIdClaim == null || !int.TryParse( userIdClaim.Value, out int userId ) )
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        context.HttpContext.Items[ "userId" ] = userId;
    }
}
