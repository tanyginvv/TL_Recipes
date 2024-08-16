using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Recipes.Domain.Entities;

namespace Recipes.Infrastructure.Entities.Users;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure( EntityTypeBuilder<User> builder )
    {
        builder.ToTable( nameof( User ) ).HasKey( t => t.Id );

        builder.Property( t => t.Id )
            .IsRequired();

        builder.Property( t => t.Name )
            .IsRequired()
            .HasMaxLength( 50 );

        builder.Property( t => t.Description )
            .IsRequired()
            .HasMaxLength( 200 );

        builder.Property( t => t.Login )
            .IsRequired()
            .HasMaxLength( 50 );

        builder.Property( t => t.PasswordHash )
            .IsRequired()
            .HasMaxLength( 250 );

        builder.HasMany( u => u.Recipes )
            .WithOne( r => r.User )
            .HasForeignKey( i => i.UserId )
            .OnDelete( DeleteBehavior.Cascade );

        builder.HasOne( u => u.AuthorizationToken )
            .WithOne( ua => ua.User )
            .HasForeignKey<UserAuthorizationToken>( ua => ua.UserId )
            .IsRequired();
    }
}