using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.UseCases.Favourites.Command.CreateFavourite;
using Recipes.Application.UseCases.Favourites.Command.DeleteFavourite;

namespace Recipes.Application.UseCases.Favourites;

public static class FavouriteBindings
{
    public static IServiceCollection AddFavouriteBindings( this IServiceCollection services )
    {
        services.AddScoped<ICommandHandler<CreateFavouriteCommand>, CreateFavouriteCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteFavouriteCommand>, DeleteFavouriteCommandHandler>();

        services.AddScoped<IAsyncValidator<CreateFavouriteCommand>, CreateFovouriteCommandValidator>();
        services.AddScoped<IAsyncValidator<DeleteFavouriteCommand>, DeleteFavouriteCommandValidator>();

        return services;
    }
}