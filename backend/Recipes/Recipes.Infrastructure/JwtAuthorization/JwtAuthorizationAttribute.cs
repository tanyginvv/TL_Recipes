using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.Tokens.DecodeToken;
using Recipes.Application.Tokens.VerificationToken;
using Recipes.Application.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Infrastructure.JwtAuthorizations
{
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

            TokenSignatureVerificator tokenSignatureVerificator = new TokenSignatureVerificator( accessToken, configuration.GetSecret() );

            tokenSignatureVerificator.VerifySignature();
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

        }
    }
}