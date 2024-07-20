using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.UseCases.Tags.Commands.CreateTag.CreateTagCommand;
using Recipes.Application.UseCases.Tags.Dtos;
using Recipes.Application.UseCases.Tags.Queries.GetRandomTags;
using Recipes.Application.UseCases.Tags.Queries.GetTagByName;
using Recipes.Application.UseCases.Tags.Queries.GetTagsByRecipeIdQuery;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Tags
{
    public static class TagBindings
    {
        public static IServiceCollection AddTagsBindings( this IServiceCollection services )
        {
            services.AddScoped<ICommandHandler<CreateTagCommand>, CreateTagCommandHandler>();

            services.AddScoped<IQueryHandler<GetTagsByRecipeIdQueryDto, GetTagsByRecipeIdQuery>, GetTagsByRecipeIdQueryHandler>();
            services.AddScoped<IQueryHandler<GetTagByNameQueryDto, GetTagByNameQuery>, GetTagByNameQueryHandler>();
            services.AddScoped<IQueryHandler<IReadOnlyList<Tag>, GetRandomTagsQuery>, GetRandomTagsQueryHandler>();

            services.AddScoped<IAsyncValidator<CreateTagCommand>, CreateTagCommandValidator>();

            services.AddScoped<IAsyncValidator<GetTagsByRecipeIdQuery>, GetTagsByRecipeIdQueryValidator>();
            services.AddScoped<IAsyncValidator<GetTagByNameQuery>, GetTagByNameQueryValidator>();
            services.AddScoped<IAsyncValidator<GetRandomTagsQuery>, GetRandomTagsQueryValidator>();

            return services;
        }
    }
}
