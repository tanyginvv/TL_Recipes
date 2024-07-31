using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Recipes.Application.Tokens;
using System.Text;

namespace Recipes.WebApi
{
    public static class ApiExtensions
    {
        public static void AddApiAuthentication(
        this IServiceCollection services )
        {
            services.AddAuthentication( JwtBearerDefaults.AuthenticationScheme )
                .AddJwtBearer( JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    // Resolve ITokenConfiguration from DI
                    var serviceProvider = services.BuildServiceProvider();
                    var tokenConfiguration = serviceProvider.GetRequiredService<ITokenConfiguration>();

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes( tokenConfiguration.GetSecret() ) )
                    };
                } );

            services.AddAuthorization();
        }
    }
}
