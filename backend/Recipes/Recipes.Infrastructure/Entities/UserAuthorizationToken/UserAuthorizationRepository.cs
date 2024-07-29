using Microsoft.EntityFrameworkCore;
using Recipes.Application.Repositories;
using Recipes.Domain.Entities;
using Recipes.Infrastructure.Context;
using System.Linq.Expressions;

namespace Infrastructure.Entities.UserAuthorizationTokens
{
    public class UserAuthorizationRepository : BaseRepository<UserAuthorizationToken>, IUserAuthorizationTokenRepository
    {
        public UserAuthorizationRepository( RecipesDbContext dbContext ) : base( dbContext )
        {
        }

        public async Task<bool> ContainsAsync( Expression<Func<UserAuthorizationToken, bool>> predicate )
        {
            return await _dbSet.Where( predicate ).FirstOrDefaultAsync() != null;
        }

        public async Task Delete( UserAuthorizationToken entity )
        {
            UserAuthorizationToken token = await GetByRefreshTokenAsync( entity.RefreshToken );
            if ( token is not null )
            {
                base.Remove( entity );
            }
        }

        public async Task<UserAuthorizationToken> GetByRefreshTokenAsync( string refreshToken )
        {
            return await _dbSet.Where( ua => ua.RefreshToken == refreshToken ).FirstOrDefaultAsync();
        }

        public async Task<UserAuthorizationToken> GetByUserIdAsync( long userId )
        {
            return await _dbSet.Where( ua => ua.UserId == userId ).FirstOrDefaultAsync();
        }
    }
}