using Microsoft.EntityFrameworkCore;
using Recipes.Application.Repositories;
using Recipes.Infrastructure.Context;
using Recipes.Domain.Entities;
using System.Linq.Expressions;

namespace Recipes.Infrastructure.Entities.UserAuthTokens;

public class UserAuthTokenRepository( RecipesDbContext context ) : BaseRepository<UserAuthToken>( context ), IUserAuthTokenRepository
{
    public async Task<bool> ContainsAsync( Expression<Func<UserAuthToken, bool>> predicate )
    {
        return await _dbSet.Where( predicate ).FirstOrDefaultAsync() != null;
    }

    public async Task Delete( UserAuthToken entity )
    {
        UserAuthToken token = await GetByRefreshTokenAsync( entity.RefreshToken );
        if ( token is not null )
        {
            base.Remove( entity );
        }
    }

    public async Task<UserAuthToken> GetByRefreshTokenAsync( string refreshToken )
    {
        return await _dbSet
            .Where( ua => ua.RefreshToken == refreshToken )
            .FirstOrDefaultAsync();
    }

    public async Task<UserAuthToken> GetByUserIdAsync( int userId )
    {
        return await _dbSet
            .Where( ua => ua.UserId == userId )
            .FirstOrDefaultAsync();
    }
}