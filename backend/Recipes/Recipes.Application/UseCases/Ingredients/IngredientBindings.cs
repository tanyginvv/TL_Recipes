using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.UseCases.Ingredients.Commands.CreateIngredient;
using Recipes.Application.UseCases.Ingredients.Commands.DeleteIngredient;
using Recipes.Application.UseCases.Ingredients.Commands.UpdateIngredient;
using Recipes.Application.UseCases.Ingredients.Commands.UpdateIngredients;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Ingredients;

public static class IngredientBindings
{
    public static IServiceCollection AddIngredientsBindings( this IServiceCollection services )
    {
        services.AddScoped<ICommandHandlerWithResult<CreateIngredientCommand, Ingredient>, CreateIngredientCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateIngredientCommand>, UpdateIngredientCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteIngredientCommand>, DeleteIngredientCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateIngredientsCommand>, UpdateIngredientsCommandHandler>();

        services.AddScoped<IAsyncValidator<CreateIngredientCommand>, CreateIngredientCommandValidator>();
        services.AddScoped<IAsyncValidator<DeleteIngredientCommand>, DeleteIngredientCommandValidator>();
        services.AddScoped<IAsyncValidator<UpdateIngredientCommand>, UpdateIngredientCommandValidator>();
        services.AddScoped<IAsyncValidator<UpdateIngredientsCommand>, UpdateIngredientsCommandValidator>();


        IngredientMappingConfig.RegisterMappings();

        return services;
    }
}