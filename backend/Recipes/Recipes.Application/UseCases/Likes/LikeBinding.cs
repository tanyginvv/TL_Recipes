using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.UseCases.Likes.Command.CreateLike;
using Recipes.Application.UseCases.Likes.Command.DeleteLike;

namespace Recipes.Application.UseCases.Likes;

public static class LikeBindings
{
    public static IServiceCollection AddLikesBindings( this IServiceCollection services )
    {
        services.AddScoped<ICommandHandler<CreateLikeCommand>, CreateLikeCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteLikeCommand>, DeleteLikeCommandHandler>();


        services.AddScoped<IAsyncValidator<CreateLikeCommand>, CreateLikeCommandValidator>();
        services.AddScoped<IAsyncValidator<DeleteLikeCommand>, DeleteLikeCommandValidator>();

        return services;
    }
}