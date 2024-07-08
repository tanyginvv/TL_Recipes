using Microsoft.EntityFrameworkCore;
using Recipes.Domain.Entities;
using Recipes.Infrastructure.Entities.Recipes;

namespace Recipes.Infrastructure.Context
{
    public class RecipesDbContext : DbContext
    {
        public RecipesDbContext( DbContextOptions<RecipesDbContext> options )
            : base( options )
        { }

        protected override void OnModelCreating( ModelBuilder modelBuilder )
        {
            modelBuilder.ApplyConfiguration( new RecipeConfiguration() );
        }
    }
}
