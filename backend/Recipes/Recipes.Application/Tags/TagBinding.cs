using Application.CQRSInterfaces;
using Application.Validation;
using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Tags.Commands.CreateTag;
using Recipes.Application.Tags.Dtos;
using Recipes.Application.Tags.Queries.GetTagsByRecipeIdQuery;

namespace Recipes.Application.Tags
{
    public static class TagBindings
    {
        public static IServiceCollection AddTagsBindings( this IServiceCollection services )
        {
            services.AddScoped<ICommandHandler<CreateTagDto>, CreateTagCommandHandler>();

            services.AddScoped<IQueryHandler<GetTagsByRecipeIdQueryDto, GetTagsByRecipeIdQueryDto>, GetTagsByRecipeIdQueryHandler>();

            services.AddScoped<IAsyncValidator<CreateTagDto>, CreateTagCommandValidator>();

            services.AddScoped<IAsyncValidator<GetTagsByRecipeIdQuery>, GetTagsByRecipeIdQueryValidator>();

            return services;
        }
    }
}
