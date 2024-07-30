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

            string accessToken = context.HttpContext.Request.Cookies[ "AccessToken" ];
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
            DateTime expDate = new DateTime( 1970, 1, 1 ).AddSeconds( ( token.Payload.Exp.Value ) ).AddHours( 3 );

            if ( DateTime.UtcNow > expDate )
            {
                context.Result = new ForbidResult();
                return;
            }

            //string userIdStr = context.HttpContext.Request.Headers[ "userId" ].FirstOrDefault() ??
            //                   context.HttpContext.Request.Query[ "userId" ].FirstOrDefault();
            //if ( string.IsNullOrEmpty( userIdStr ) || !long.TryParse( userIdStr, out long requestUserId ) )
            //{
            //    context.Result = new ForbidResult();
            //    return;
            //}

            if ( !long.TryParse( token.Payload[ "userId" ]?.ToString(), out long tokenUserId ) /*|| requestUserId != tokenUserId*/
                )
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}