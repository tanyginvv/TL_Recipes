using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.Interfaces;
using Recipes.Application.UseCases.Services.AuthService;

namespace Recipes.Application.UseCases.Services;
public static class ServicesBindings
{
    public static IServiceCollection AddServiceBindings( this IServiceCollection services )
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        return services;
    }
}