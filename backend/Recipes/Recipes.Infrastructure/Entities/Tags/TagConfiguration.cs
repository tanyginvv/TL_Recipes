using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Recipes.Domain.Entities;

namespace Recipes.Infrastructure.Entities.Tags
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure( EntityTypeBuilder<Tag> builder )
        {
            builder.ToTable( nameof( Tag ) ).HasKey( t => t.Id );

            builder.Property( t => t.Id )
                .IsRequired();

            builder.Property( t => t.Name )
                .IsRequired()
                .HasMaxLength( 50 );
        }
    }
}