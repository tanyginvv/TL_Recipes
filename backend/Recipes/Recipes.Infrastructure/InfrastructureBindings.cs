using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Recipes.Application.Repositories;
using Recipes.Application.Interfaces;
using Recipes.Infrastructure.ImageTools;
using Recipes.Application.Tokens.CreateToken;
using Recipes.Application.Tokens.DecodeToken;
using Recipes.Application.Tokens.VerificationToken;
using Recipes.Application.PasswordHasher;
using Recipes.Infrastructure.PasswordHashers;
using Recipes.Infrastructure.TokenUtils.DecodeToken;
using Recipes.Infrastructure.TokenUtils.CreateToken;
using Recipes.Infrastructure.TokenUtils.VerificationToken;
using Recipes.Infrastructure.DataAccess;
using Recipes.Infrastructure.DataAccess.Ingredients;
using Recipes.Infrastructure.DataAccess.Recipes;
using Recipes.Infrastructure.DataAccess.UserAuthTokens;
using Recipes.Infrastructure.DataAccess.Users;
using Recipes.Infrastructure.DataAccess.Tags;
using Recipes.Infrastructure.DataAccess.Steps;
using Recipes.Application.Tokens;
using Recipes.Infrastructure.Options;
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
        services.AddScoped<IUserAuthTokenRepository, UserAuthTokenRepository>();
        services.AddScoped<IImageTools, FileImageTools>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        services.AddScoped<ITokenConfiguration, TokenConfiguration>();
        services.AddScoped<ITokenCreator, TokenCreator>();
        services.AddScoped<ITokenDecoder, TokenDecoder>();
        services.AddScoped<ITokenSignatureVerificator, TokenSignatureVerificator>();

        return services;
    }
}