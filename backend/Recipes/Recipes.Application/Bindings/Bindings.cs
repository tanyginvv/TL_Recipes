using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.Tags;
using Recipes.Application.Steps;
using Recipes.Application.Recipes;
using Recipes.Application.Ingredients;
using Recipes.Infrastructure.Repositories;
using Recipes.Infrastructure.Entities.Tags;
using Recipes.Infrastructure.Context;
using Recipes.Infrastructure.Entities.Recipes;
using Recipes.Infrastructure.Entities.Steps;

namespace Recipes.Application
{
    public static class Bindings
    {
        public static IServiceCollection AddBindings( this IServiceCollection services )
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRecipeRepository, RecipeRepository>();
            services.AddScoped<IIngredientRepository, IngredientRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IStepRepository, StepRepository>();

            services.AddRecipesBindings();
            services.AddTagsBindings();
            services.AddStepsBindings();
            services.AddIngredientsBindings();

            return services;
        }
    }
}
