using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.UseCases.Favourites.Dtos;
using Recipes.Application.UseCases.Likes.Command;
using Recipes.Application.UseCases.Likes.Dtos;
using Recipes.Application.UseCases.Likes.Queries.GetLikeBoolRecipeAndUser;
using Recipes.Application.UseCases.Likes.Queries.GetLikesCountForRecipeQuery;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Favourites;

public static class FavouriteBindings
{
    public static IServiceCollection AddFavouriteBindings( this IServiceCollection services )
    {
        services.AddScoped<ICommandHandler<CreateFavouriteCommand>, CreateFavouriteCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteFavouriteCommand>, DeleteFavouriteCommandHandler>();

        services.AddScoped<IQueryHandler<FavouritesCountDto, GetFavouritesCountForRecipeQuery>, GetFavouritesCountForRecipeQueryHandler>();
        services.AddScoped<IQueryHandler<FavouriteBoolDto, GetFavouriteBoolRecipeAndUserQuery>, GetFavouriteBoolRecipeAndUserQueryHandler>();

        services.AddScoped<IAsyncValidator<CreateFavouriteCommand>, CreateFovouriteCommandValidator>();
        services.AddScoped<IAsyncValidator<DeleteFavouriteCommand>, DeleteFavouriteCommandValidator>();

        services.AddScoped<IAsyncValidator<GetFavouritesCountForRecipeQuery>, GetFavouritesCountForRecipeQueryValidator>();
        services.AddScoped<IAsyncValidator<GetFavouriteBoolRecipeAndUserQuery>, GetFavouriteBoolRecipeAndUserQueryValidator>();
        return services;
    }
}
