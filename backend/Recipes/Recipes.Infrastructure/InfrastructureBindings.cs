using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Recipes.Infrastructure.Context;
using Recipes.Application.Repositories;
using Recipes.Infrastructure.Entities.Recipes;
using Recipes.Infrastructure.Entities.Steps;
using Recipes.Infrastructure.Entities.Tags;
using Recipes.Infrastructure.Entities.Ingredients;
using Recipes.Application.Interfaces;
using Recipes.Infrastructure.ImageTools;
using Recipes.Infrastructure.Entities.Users;
using Infrastructure.Entities.UserAuthorizationTokens;
using Infrastructure.ConfigurationUtils.Token;
using Recipes.Application.Tokens;
using Recipes.Application.PasswordHasher;

namespace Recipes.Infrastructure
{
    public static class InfrastructureBindings
    {
        public static IServiceCollection AddInfrastructureBindings( this IServiceCollection services, IConfiguration configuration )
        {
            services.AddDbContext<RecipesDbContext>( options =>
                    options.UseSqlServer( configuration.GetConnectionString( "Recipes" ),
                    db => db.MigrationsAssembly( "Infrastructure.Migration" ) ) );

            services.AddScoped<ITokenConfiguration, TokenConfiguration>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRecipeRepository, RecipeRepository>();
            services.AddScoped<IIngredientRepository, IngredientRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IStepRepository, StepRepository>();
            services.AddScoped<IImageTools, ImageHelperTools>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserAuthorizationTokenRepository, UserAuthorizationRepository>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            return services;
        }
    }
}
