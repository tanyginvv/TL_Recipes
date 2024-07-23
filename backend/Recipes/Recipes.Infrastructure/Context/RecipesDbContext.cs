using Microsoft.EntityFrameworkCore;
using Recipes.Domain.Entities;
using Recipes.Infrastructure.Entities.Ingredients;
using Recipes.Infrastructure.Entities.Recipes;
using Recipes.Infrastructure.Entities.Steps;
using Recipes.Infrastructure.Entities.Tags;

namespace Recipes.Infrastructure.Context
{
    public class RecipesDbContext : DbContext
    {
        public RecipesDbContext( DbContextOptions<RecipesDbContext> options )
           : base( options )
        { }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Step> Steps { get; set; }

        protected override void OnModelCreating( ModelBuilder modelBuilder )
        {
            base.OnModelCreating( modelBuilder );

            modelBuilder.HasSequence<int>( "RecipeHiLo", schema: "dbo" )
               .StartsAt( 1 )
               .IncrementsBy( 10 );

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
                    j => j.HasOne<Recipe>().WithMany().HasForeignKey( "RecipeId" )
                );
        }
    }
}
