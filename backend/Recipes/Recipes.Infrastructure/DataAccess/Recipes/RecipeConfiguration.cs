using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recipes.Domain.Entities;

namespace Recipes.Infrastructure.DataAccess.Recipes;

public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure( EntityTypeBuilder<Recipe> builder )
    {
        builder.ToTable( nameof( Recipe ) ).HasKey( r => r.Id );

        builder.Property( r => r.Id )
            .IsRequired();

        builder.Property( r => r.AuthorId )
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

        builder.HasMany( r => r.Ingredients )
            .WithOne( i => i.Recipe )
            .HasForeignKey( i => i.RecipeId )
            .OnDelete( DeleteBehavior.Cascade );

        builder.HasMany( r => r.Steps )
            .WithOne( s => s.Recipe )
            .HasForeignKey( s => s.RecipeId )
            .OnDelete( DeleteBehavior.Cascade );

        builder.HasMany( r => r.Likes )
            .WithOne( l => l.Recipe )
            .HasForeignKey( l => l.RecipeId )
            .OnDelete( DeleteBehavior.Cascade );

        builder.HasMany( r => r.Favourites )
            .WithOne( f => f.Recipe )
            .HasForeignKey( f => f.RecipeId )
            .OnDelete( DeleteBehavior.Cascade );
    }
}