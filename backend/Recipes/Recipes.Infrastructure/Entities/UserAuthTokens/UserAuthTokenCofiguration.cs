using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recipes.Domain.Entities;

namespace Recipes.Infrastructure.Entities.UserAuthTokens;

public class UserAuthTokenConfiguration : IEntityTypeConfiguration<UserAuthToken>
{
    public void Configure( EntityTypeBuilder<UserAuthToken> builder )
    {
        builder.ToTable( "UserAuthToken" )
            .HasKey( ua => ua.UserId );

        builder.Property( ua => ua.UserId ).IsRequired();
        builder.Property( ua => ua.RefreshToken ).IsRequired().HasMaxLength( 40 );
        builder.Property( ua => ua.ExpiryDate ).IsRequired();
    }
}