﻿using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Application.UseCases.Tags.Commands;
using Recipes.Application.UseCases.Tags.Dtos;
using Recipes.Application.UseCases.Tags.Queries.GetTagByName;
using Recipes.Application.UseCases.Tags.Queries.GetTagsByRecipeIdQuery;
using Recipes.Application.UseCases.Tags.Queries.GetTagsForSearch;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Tags
{
    public static class TagBindings
    {
        public static IServiceCollection AddTagsBindings( this IServiceCollection services )
        {
            services.AddScoped<ICommandHandlerWithResult<GetOrCreateTagCommand, Tag>, GetOrCreateTagCommandHandler>();

            services.AddScoped<IQueryHandler<GetTagsByRecipeIdQueryDto, GetTagsByRecipeIdQuery>, GetTagsByRecipeIdQueryHandler>();
            services.AddScoped<IQueryHandler<GetTagByNameQueryDto, GetTagByNameQuery>, GetTagByNameQueryHandler>();
            services.AddScoped<IQueryHandler<IReadOnlyList<TagDto>, GetTagsForSearchQuery>, GetTagsForSearchQueryHandler>();

            services.AddScoped<IAsyncValidator<GetOrCreateTagCommand>, GetOrCreateTagCommandValidator>();

            services.AddScoped<IAsyncValidator<GetTagsByRecipeIdQuery>, GetTagsByRecipeIdQueryValidator>();
            services.AddScoped<IAsyncValidator<GetTagByNameQuery>, GetTagByNameQueryValidator>();
            services.AddScoped<IAsyncValidator<GetTagsForSearchQuery>, GetRandomTagsQueryValidator>();

            return services;
        }
    }
}
