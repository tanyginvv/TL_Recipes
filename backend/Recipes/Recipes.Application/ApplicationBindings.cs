using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.UseCases.Tags;
using Recipes.Application.UseCases.Steps;
using Recipes.Application.UseCases.Ingredients;
using Recipes.Application.UseCases.Recipes;
using Recipes.Application.UseCases.Users;
using Application.UserAuthorizationTokens;
using Recipes.Application.UseCases.Likes;
using Recipes.Application.UseCases.Favourites;

namespace Recipes.Application;

public static class ApplicationBindings
{
    public static IServiceCollection AddApplicationBindings( this IServiceCollection services )
    {
        services.AddRecipesBindings();
        services.AddTagsBindings();
        services.AddStepsBindings();
        services.AddIngredientsBindings();
        services.AddUserBindings();
        services.AddUserAuthorizationTokenBindings();
        services.AddLikesBindings();
        services.AddFavouriteBindings();

        return services;
    }
}
