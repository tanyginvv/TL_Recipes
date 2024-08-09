using Application.UseCases.UserAuthorizationTokens;
using Application.UserAuthorizationTokens.Commands.RefreshToken;
using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.UseCases.UserAuthorizationTokens.Dto;
using Recipes.Application.UseCases.UserAuthorizationTokens.RefreshToken;
using Recipes.Application.Validation;

namespace Application.UserAuthorizationTokens;

public static class UserAuthorizationTokenBindings
{
    public static IServiceCollection AddUserAuthorizationTokenBindings( this IServiceCollection services )
    {
        services.AddScoped<ICommandHandlerWithResult<RefreshTokenCommand, RefreshTokenCommandDto>, RefreshTokenCommandHandler>();
        services.AddScoped<IAsyncValidator<RefreshTokenCommand>, RefreshTokenCommandValidator>();

        return services;
    }
}