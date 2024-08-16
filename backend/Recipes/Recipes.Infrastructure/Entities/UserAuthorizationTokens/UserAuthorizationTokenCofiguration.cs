using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recipes.Domain.Entities;

namespace Infrastructure.Entities.UserAuthorizationTokens;

public class UserAuthorizationTokenConfiguration : IEntityTypeConfiguration<UserAuthorizationToken>
{
    public void Configure( EntityTypeBuilder<UserAuthorizationToken> builder )
    {
        builder.ToTable( "UserAuthorizationToken" )
            .HasKey( ua => ua.UserId );

        builder.Property( ua => ua.UserId ).IsRequired();
        builder.Property( ua => ua.RefreshToken ).IsRequired().HasMaxLength( 40 );
        builder.Property( ua => ua.ExpiryDate ).IsRequired();
    }
}