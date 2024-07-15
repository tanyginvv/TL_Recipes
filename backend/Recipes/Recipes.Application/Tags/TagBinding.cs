using Application.CQRSInterfaces;
using Application.Validation;
using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Tags.Commands.CreateTag;
using Recipes.Application.Tags.Dtos;
using Recipes.Application.Tags.Queries.GetTagsByName;
using Recipes.Application.Tags.Queries.GetTagsByRecipeIdQuery;
using Recipes.Recipes.Infrastructure.Repositories.Tags.Commands.CreateTag;

namespace Recipes.Application.Tags
{
    public static class TagBindings
    {
        public static IServiceCollection AddTagsBindings( this IServiceCollection services )
        {
            services.AddScoped<ICommandHandler<CreateTagCommand>, CreateTagCommandHandler>();

            services.AddScoped<IQueryHandler<GetTagsByRecipeIdQueryDto, GetTagsByRecipeIdQuery>, GetTagsByRecipeIdQueryHandler>();
            services.AddScoped<IQueryHandler<GetTagByNameQueryDto, GetTagByNameQuery>, GetTagByNameQueryHandler>();

            services.AddScoped<IAsyncValidator<CreateTagCommand>, CreateTagCommandValidator>();

            services.AddScoped<IAsyncValidator<GetTagsByRecipeIdQuery>, GetTagsByRecipeIdQueryValidator>();
            services.AddScoped<IAsyncValidator<GetTagByNameQuery>, GetTagByNameQueryValidator>();

            return services;
        }
    }
}
