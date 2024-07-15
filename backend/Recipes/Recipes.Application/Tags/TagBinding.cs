using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Tags.Commands.CreateTag.CreateTagCommand;
using Recipes.Application.Tags.Dtos;
using Recipes.Application.Tags.Queries.GetTagByName;
using Recipes.Application.Tags.Queries.GetTagsByRecipeIdQuery;
using Recipes.Application.Validation;

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
