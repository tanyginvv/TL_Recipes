using Microsoft.EntityFrameworkCore;
using Recipes.Domain.Entities;
using Recipes.Infrastructure.DataAccess.Ingredients;
using Recipes.Infrastructure.DataAccess.Recipes;
using Recipes.Infrastructure.DataAccess.Steps;
using Recipes.Infrastructure.DataAccess.Tags;
using Recipes.Infrastructure.DataAccess.UserAuthTokens;
using Recipes.Infrastructure.DataAccess.Users;

namespace Recipes.Infrastructure.DataAccess;

public class RecipesDbContext( DbContextOptions<RecipesDbContext> options ) : DbContext( options )
{
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Step> Steps { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserAuthToken> UserAuthTokens { get; set; }

    protected override void OnModelCreating( ModelBuilder modelBuilder )
    {
        base.OnModelCreating( modelBuilder );

        modelBuilder.ApplyConfiguration( new RecipeConfiguration() );
        modelBuilder.ApplyConfiguration( new IngredientConfiguration() );
        modelBuilder.ApplyConfiguration( new TagConfiguration() );
        modelBuilder.ApplyConfiguration( new StepConfiguration() );


        modelBuilder.Entity<Recipe>()
            .HasMany( r => r.Tags )
            .WithMany( t => t.Recipes )
            .UsingEntity<Dictionary<string, object>>(
                "RecipeTag",
                j => j.HasOne<Tag>().WithMany().HasForeignKey( "TagId" ),
                j => j.HasOne<Recipe>().WithMany().HasForeignKey( "RecipeId" ) )
            .HasKey( "TagId", "RecipeId" ); ;

        modelBuilder.ApplyConfiguration( new UserConfiguration() );
        modelBuilder.ApplyConfiguration( new UserAuthTokenConfiguration() );
    }
}