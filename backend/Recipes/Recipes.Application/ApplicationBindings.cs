using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.UseCases.Tags;
using Recipes.Application.UseCases.Steps;
using Recipes.Application.UseCases.Ingredients;
using Recipes.Application.UseCases.Recipes;
using Recipes.Application.ImageTools;

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
            services.AddScoped<ImageHelperTools>();

            return services;
        }
    }
}
