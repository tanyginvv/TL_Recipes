using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recipes.Domain.Entities;

namespace Recipes.Infrastructure.Entities.Recipes
{
    public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
    {
        public void Configure( EntityTypeBuilder<Recipe> builder )
        {
            builder.ToTable( nameof( Recipe ) ).HasKey( r => r.Id );

            builder.Property( r => r.Id )
                .UseHiLo( "RecipeHiLo", "dbo" )
                .IsRequired();

            builder.Property( r => r.Name )
                .HasMaxLength( 100 )
                .IsRequired();

            builder.Property( r => r.Description )
                .HasMaxLength( 150 )
                .IsRequired();

            builder.Property( r => r.CookTime )
                .IsRequired();

            builder.Property( r => r.PortionCount )
                .IsRequired();

            builder.Property( r => r.ImageUrl )
                .IsRequired();
        }
    }
}