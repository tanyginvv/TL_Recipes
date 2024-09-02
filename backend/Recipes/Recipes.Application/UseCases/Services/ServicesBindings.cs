using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.Interfaces;
using Recipes.Application.UseCases.Services.AuthTokenServices;
using Recipes.Application.UseCases.Services.LoginServices;
using Recipes.Application.UseCases.Services.RefreshTokenServices;

namespace Recipes.Application.UseCases.Services;
public static class ServicesBindings
{
    public static IServiceCollection AddServiceBindings( this IServiceCollection services )
    {
        services.AddScoped<ILoginService, LoginService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<IAuthTokenService, AuthTokenService>();

        return services;
    }
}