using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Recipes.Domain.Entities;

namespace Recipes.Infrastructure.DataAccess.Users;

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
            .WithOne( r => r.Author )
            .HasForeignKey( i => i.AuthorId )
            .OnDelete( DeleteBehavior.Restrict );

        builder.HasOne( u => u.AuthToken )
            .WithOne( ua => ua.User )
            .HasForeignKey<UserAuthToken>( ua => ua.UserId )
            .IsRequired();

        builder.HasMany( u => u.Likes )
            .WithOne( ua => ua.User )
            .HasForeignKey( ua => ua.UserId )
            .OnDelete( DeleteBehavior.Restrict );

        builder.HasMany( u => u.Favourites )
            .WithOne( ua => ua.User )
            .HasForeignKey( ua => ua.UserId )
            .OnDelete( DeleteBehavior.Restrict );
    }
}