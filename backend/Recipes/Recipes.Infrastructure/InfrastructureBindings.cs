using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Recipes.Infrastructure.Repositories;
using Recipes.Infrastructure.Context;
using Recipes.Application.Repositories;
using Recipes.Infrastructure.Entities.Recipes;
using Recipes.Infrastructure.Entities.Steps;
using Recipes.Infrastructure.Entities.Tags;

namespace Recipes.Application
{
    public static class InfrastructureBindings
    {
        public static IServiceCollection AddInfrastructureBindings( this IServiceCollection services, IConfiguration configuration )
        {
            services.AddDbContext<RecipesDbContext>( options =>
                    options.UseSqlServer( configuration.GetConnectionString( "Recipes" ) ) );

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRecipeRepository, RecipeRepository>();
            services.AddScoped<IIngredientRepository, IngredientRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IStepRepository, StepRepository>();

            return services;
        }
    }
}
