using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Recipes.Application.Repositories;
using Recipes.Domain.Entities;

namespace Recipes.Infrastructure.DataAccess.Likes;

public class LikeRepository( RecipesDbContext context ) : BaseRepository<Like>( context ), ILikeRepository
{
    public async Task Delete( Like like )
    {
        Like foundLike = await GetByIdAsync( like.Id );
        if ( foundLike is not null )
        {
            base.Remove( foundLike );
        }
    }

    public async Task<Like> GetLikeByAttributes( int recipeId, int userId )
    {
        return await _dbSet
            .FirstOrDefaultAsync( r => r.RecipeId == recipeId && r.UserId == userId );
    }
}