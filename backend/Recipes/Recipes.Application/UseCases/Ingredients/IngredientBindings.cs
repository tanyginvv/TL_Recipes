﻿using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.UseCases.Ingredients.Commands.CreateIngredient;
using Recipes.Application.UseCases.Ingredients.Commands.DeleteIngredient;
using Recipes.Application.UseCases.Ingredients.Commands.UpdateIngredient;
using Recipes.Application.UseCases.Ingredients.Dtos;
using Recipes.Application.UseCases.Ingredients.Queries.GetIngredientsByRecipeIdQuery;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Ingredients
{
    public static class IngredientBindings
    {
        public static IServiceCollection AddIngredientsBindings( this IServiceCollection services )
        {
            services.AddScoped<ICommandHandler<CreateIngredientCommand>, CreateIngredientCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateIngredientCommand>, UpdateIngredientCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteIngredientCommand>, DeleteIngredientCommandHandler>();

            services.AddScoped<IQueryHandler<GetIngredientsByRecipeIdQueryDto, GetIngredientsByRecipeIdQuery>, GetIngredientsByRecipeIdQueryHandler>();

            services.AddScoped<IAsyncValidator<CreateIngredientCommand>, CreateIngredientCommandValidator>();
            services.AddScoped<IAsyncValidator<DeleteIngredientCommand>, DeleteIngredientCommandValidator>();
            services.AddScoped<IAsyncValidator<UpdateIngredientCommand>, UpdateIngredientCommandValidator>();

            services.AddScoped<IAsyncValidator<GetIngredientsByRecipeIdQuery>, GetIngredientsByRecipeIdQueryValidator>();

            return services;
        }
    }
}
