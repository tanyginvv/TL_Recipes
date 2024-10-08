﻿using Recipes.Application.CQRSInterfaces;
using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Application.UseCases.Recipes.Commands.UpdateRecipe;
using Recipes.Application.UseCases.Recipes.Queries.GetRecipeById;
using Recipes.Application.UseCases.Recipes.Commands.CreateRecipe;
using Recipes.Application.UseCases.Recipes.Commands.DeleteRecipe;
using Recipes.Application.UseCases.Recipes.Queries.GetRecipes;
using Recipes.Application.UseCases.Tags.Commands.UpdateRecipeTags;
using Recipes.Application.UseCases.Recipes.Queries.GetRecipeOfDay;

namespace Recipes.Application.UseCases.Recipes;

public static class RecipesBindings
{
    public static IServiceCollection AddRecipesBindings( this IServiceCollection services )
    {
        services.AddScoped<ICommandHandlerWithResult<CreateRecipeCommand, RecipeIdDto>, CreateRecipeCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateRecipeCommand>, UpdateRecipeCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteRecipeCommand>, DeleteRecipeCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateTagsCommand>, UpdateTagsCommandHandler>();

        services.AddScoped<IQueryHandler<GetRecipeQueryDto, GetRecipeByIdQuery>, GetRecipeByIdQueryHandler>();
        services.AddScoped<IQueryHandler<GetRecipesListDto, GetRecipesQuery>, GetRecipesQueryHandler>();
        services.AddScoped<IQueryHandler<GetRecipeOfDayDto, GetRecipeOfDayQuery>, GetRecipeOfDayQueryHandler>();

        services.AddScoped<IAsyncValidator<CreateRecipeCommand>, CreateRecipeCommandValidator>();
        services.AddScoped<IAsyncValidator<DeleteRecipeCommand>, DeleteRecipeCommandValidator>();
        services.AddScoped<IAsyncValidator<UpdateRecipeCommand>, UpdateRecipeCommandValidator>();
        services.AddScoped<IAsyncValidator<UpdateTagsCommand>, UpdateTagsCommandValidator>();

        services.AddScoped<IAsyncValidator<GetRecipeByIdQuery>, GetRecipeByIdQueryValidator>();
        services.AddScoped<IAsyncValidator<GetRecipesQuery>, GetRecipesQueryValidator>();
        services.AddScoped<IAsyncValidator<GetRecipeOfDayQuery>, GetRecipeOfDayQueryValidator>();

        RecipeMappingConfig.RegisterMappings();

        return services;
    }
}