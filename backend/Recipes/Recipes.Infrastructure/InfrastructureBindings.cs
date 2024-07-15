using Microsoft.Extensions.DependencyInjection;
using Recipes.Infrastructure.Repositories;
using Recipes.Infrastructure.Entities.Tags;
using Recipes.Infrastructure.Context;
using Recipes.Infrastructure.Entities.Recipes;
using Recipes.Infrastructure.Entities.Steps;
using Recipes.Application.Repositories;

namespace Recipes.Application
{
    public static class InfrastructureBindings
    {
        public static IServiceCollection AddInfrastructureBindings( this IServiceCollection services )
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRecipeRepository, RecipeRepository>();
            services.AddScoped<IIngredientRepository, IngredientRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IStepRepository, StepRepository>();

            return services;
        }
    }
}
