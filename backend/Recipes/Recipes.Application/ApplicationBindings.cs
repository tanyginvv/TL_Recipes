using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.Tags;
using Recipes.Application.Steps;
using Recipes.Application.Recipes;
using Recipes.Application.Ingredients;

namespace Recipes.Application
{
    public static class ApplicationBindings
    {
        public static IServiceCollection AddApplicationBindings( this IServiceCollection services )
        {
            services.AddRecipesBindings();
            services.AddTagsBindings();
            services.AddStepsBindings();
            services.AddIngredientsBindings();

            return services;
        }
    }
}
