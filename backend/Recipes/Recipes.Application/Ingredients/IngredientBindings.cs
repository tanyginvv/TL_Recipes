using Application.CQRSInterfaces;
using Application.Validation;
using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Ingredients.Commands;
using Recipes.Application.Ingredients.Commands.CreateIngredient;
using Recipes.Application.Ingredients.Commands.DeleteIngredient;
using Recipes.Application.Ingredients.Commands.UpdateIngredient;
using Recipes.Application.Ingredients.Dtos;
using Recipes.Application.Ingredients.Queries;
using Recipes.Application.Ingredients.Queries.GetIngredientsByRecipeId;

namespace Recipes.Application.Ingredients
{
    public static class IngredientBindings
    {
        public static IServiceCollection AddIngredientsBindings( this IServiceCollection services )
        {
            services.AddScoped<ICommandHandler<CreateIngredientCommand>, CreateIngredientCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateIngredientCommand>, UpdateIngredientCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteIngredientCommand>, DeleteIngredientCommandHandler>();

            services.AddScoped<IQueryHandler<GetIngredientsByRecipeIdQueryDto, GetIngredientsByRecipeIdQueryDto>, GetIngredientsByRecipeIdQueryHandler>();

            services.AddScoped<IAsyncValidator<CreateIngredientCommand>, CreateIngredientCommandValidator>();
            services.AddScoped<IAsyncValidator<DeleteIngredientCommand>, DeleteIngredientCommandValidator>();
            services.AddScoped<IAsyncValidator<UpdateIngredientCommand>, UpdateIngredientCommandValidator>();

            services.AddScoped<IAsyncValidator<GetIngredientsByRecipeIdQueryDto>, GetIngredientsByRecipeIdQueryValidator>();

            return services;
        }
    }
}
