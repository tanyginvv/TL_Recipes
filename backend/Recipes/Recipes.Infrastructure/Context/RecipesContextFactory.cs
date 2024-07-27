using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Recipes.Infrastructure.Context
{
    public class RecipesContextFactory : IDesignTimeDbContextFactory<RecipesDbContext>
    {
        public RecipesDbContext CreateDbContext( string[] args )
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath( Directory.GetCurrentDirectory() )
                .AddJsonFile( "appsettings.json" )
                .AddJsonFile( $"appsettings.{Environment.GetEnvironmentVariable( "ASPNETCORE_ENVIRONMENT" )}.json", true )
                .AddEnvironmentVariables();

            var config = builder.Build();
            var connectionString = config.GetConnectionString( "Recipes" );
            var optionsBuilder = new DbContextOptionsBuilder<RecipesDbContext>();
            optionsBuilder.UseSqlServer( connectionString );

            return new RecipesDbContext( optionsBuilder.Options );
        }
    }
}