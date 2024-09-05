using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Recipes.Application.Repositories;
using Recipes.Domain.Entities;

namespace Recipes.Infrastructure.DataAccess.Favourites;

public class FavouriteRepository( RecipesDbContext context ) : BaseRepository<Favourite>( context ), IFavouriteRepository
{
    public async Task<Favourite> GetByIdAsync( Favourite fav )
    {
        return await _dbSet
            .FirstOrDefaultAsync( r => r.Id == fav.Id );
    }

    public async Task<bool> ContainsAsync( Expression<Func<Favourite, bool>> predicate )
    {
        return await _dbSet.AnyAsync( predicate );
    }

    public async Task Delete( Favourite fav )
    {
        Favourite foundFav = await GetByIdAsync( fav );
        if ( foundFav is not null )
        {
            base.Remove( foundFav );
        }
    }

    public async Task<Favourite> GetFavouriteByAttributes( int recipeId, int userId )
    {
        return await _dbSet
            .FirstOrDefaultAsync( r => r.RecipeId == recipeId && r.UserId == userId );
    }
}