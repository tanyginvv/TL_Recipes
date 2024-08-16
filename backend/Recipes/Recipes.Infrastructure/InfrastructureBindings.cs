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
using Recipes.Application.Tokens.CreateToken;
using Recipes.Infrastructure.Tokens.CreateToken;
using Recipes.Infrastructure.Tokens.DecodeToken;
using Recipes.Application.Tokens.DecodeToken;
using Recipes.Application.Tokens.VerificationToken;
using Recipes.Infrastructure.Tokens.VerificationToken;
using Recipes.Infrastructure.Entities.Users;
using Infrastructure.Entities.UserAuthorizationTokens;
using Recipes.Application.PasswordHasher;
using Recipes.Application.Tokens;
using Recipes.Infrastructure.ConfigurationUtils;

namespace Recipes.Infrastructure;

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
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserAuthorizationTokenRepository, UserAuthorizationTokenRepository>();
        services.AddScoped<IImageTools, ImageHelperTools>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        services.AddScoped<ITokenConfiguration, TokenConfiguration>();
        services.AddScoped<ITokenCreator, TokenCreator>();
        services.AddScoped<ITokenDecoder, TokenDecoder>();
        services.AddScoped<ITokenSignatureVerificator, TokenSignatureVerificator>();

        return services;
    }
}