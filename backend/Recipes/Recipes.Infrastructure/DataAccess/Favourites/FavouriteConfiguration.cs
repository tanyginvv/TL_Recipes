using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Recipes.Domain.Entities;

namespace Recipes.Infrastructure.DataAccess.Favourites;

public class FavouriteConfiguration : IEntityTypeConfiguration<Favourite>
{
    public void Configure( EntityTypeBuilder<Favourite> builder )
    {
        builder.ToTable( nameof( Favourite ) ).HasKey( r => r.Id );

        builder.Property( l => l.UserId )
             .IsRequired();

        builder.Property( l => l.RecipeId )
            .IsRequired();
    }
}