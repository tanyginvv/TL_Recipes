using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recipes.Domain.Entities;

namespace Recipes.Infrastructure.DataAccess.Ingredients;

public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
{
    public void Configure( EntityTypeBuilder<Ingredient> builder )
    {
        builder.ToTable( nameof( Ingredient ) ).HasKey( i => i.Id );

        builder.Property( i => i.Id )
            .IsRequired();

        builder.Property( i => i.Title )
            .IsRequired()
            .HasMaxLength( 50 );

        builder.Property( i => i.Description )
            .IsRequired()
            .HasMaxLength( 250 );

        builder.Property( i => i.RecipeId )
            .IsRequired();
    }
}