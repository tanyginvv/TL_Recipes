using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recipes.Domain.Entities;

namespace Recipes.Infrastructure.DataAccess.Likes;

public class LikeConfiguration : IEntityTypeConfiguration<Like>
{
    public void Configure( EntityTypeBuilder<Like> builder )
    {
        builder.ToTable( nameof( Like ) ).HasKey( r => r.Id );

        builder.Property( l => l.UserId )
            .IsRequired();

        builder.Property( l => l.RecipeId )
            .IsRequired();

        builder.HasIndex( l => new { l.RecipeId, l.UserId } )
           .HasDatabaseName( "IX_Likes_RecipeId_UserId" )
           .IsUnique();
    }
}