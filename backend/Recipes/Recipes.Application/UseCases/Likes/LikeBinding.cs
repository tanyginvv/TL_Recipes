using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.UseCases.Likes.Command;
using Recipes.Application.UseCases.Likes.Dtos;
using Recipes.Application.UseCases.Likes.Queries.GetLikeBoolRecipeAndUser;
using Recipes.Application.UseCases.Likes.Queries.GetLikesCountForRecipeQuery;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Likes;

public static class LikeBindings
{
    public static IServiceCollection AddLikesBindings( this IServiceCollection services )
    {
        services.AddScoped<ICommandHandler<CreateLikeCommand>, CreateLikeCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteLikeCommand>, DeleteLikeCommandHandler>();

        services.AddScoped<IQueryHandler<LikesCountDto, GetLikesCountForRecipeQuery>, GetLikesCountForRecipeQueryHandler>();
        services.AddScoped<IQueryHandler<LikeBoolDto, GetLikeBoolRecipeAndUserQuery>, GetLikeBoolRecipeAndUserQueryHandler>();

        services.AddScoped<IAsyncValidator<CreateLikeCommand>, CreateLikeCommandValidator>();
        services.AddScoped<IAsyncValidator<DeleteLikeCommand>, DeleteLikeCommandValidator>();

        services.AddScoped<IAsyncValidator<GetLikesCountForRecipeQuery>, GetLikesCountForRecipeQueryValidator>();
        services.AddScoped<IAsyncValidator<GetLikeBoolRecipeAndUserQuery>, GetLikeBoolRecipeAndUserQueryValidator>();
        return services;
    }
}
