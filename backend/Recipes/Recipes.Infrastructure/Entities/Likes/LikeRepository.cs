using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Recipes.Application.Repositories;
using Recipes.Domain.Entities;
using Recipes.Infrastructure.Context;

namespace Recipes.Infrastructure.Entities.Likes;

public class LikeRepository : BaseRepository<Like>, ILikeRepository
{
    public LikeRepository( RecipesDbContext context ) : base( context )
    {
    }

    public async Task<Like> GetByIdAsync( Like like )
    {
        return await _dbSet
            .FirstOrDefaultAsync( r => r.Id == like.Id );
    }

    public async Task AddAsync( Like like )
    {
        await base.AddAsync( like );
    }

    public async Task<bool> ContainsAsync( Expression<Func<Like, bool>> predicate )
    {
        return await _dbSet.AnyAsync( predicate );
    }

    public async Task Delete( Like like )
    {
        Like foundLike = await GetByIdAsync( like.Id );
        if ( foundLike != null )
        {
            base.Remove( foundLike );
        }
    }

    public async Task<int> GetLikesCount( int recipeId )
    {
        return await _dbSet
            .Where( r => r.RecipeId == recipeId )
            .CountAsync();
    }

    public async Task<Like> GetLikeByAttributes( int userId, int recipeId )
    {
        return await _dbSet
            .Where( r => r.RecipeId == recipeId && r.UserId == userId )
            .FirstOrDefaultAsync();
    }
}
