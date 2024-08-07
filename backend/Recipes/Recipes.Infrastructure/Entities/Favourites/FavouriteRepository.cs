using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Recipes.Application.Repositories;
using Recipes.Domain.Entities;
using Recipes.Infrastructure.Context;

namespace Recipes.Infrastructure.Entities.Favourites
{
    public class FavouriteRepository : BaseRepository<Favourite>, IFavouriteRepository
    {
        public FavouriteRepository( RecipesDbContext context ) : base( context )
        {
        }

        public async Task<Favourite> GetByIdAsync( Favourite fav )
        {
            return await _dbSet
                .FirstOrDefaultAsync( r => r.Id == fav.Id );
        }

        public async Task AddAsync( Favourite fav )
        {
            await base.AddAsync( fav );
        }

        public async Task<bool> ContainsAsync( Expression<Func<Favourite, bool>> predicate )
        {
            return await _dbSet.AnyAsync( predicate );
        }

        public async Task Delete( Favourite fav )
        {
            Favourite foundFav = await GetByIdAsync( fav );
            base.Remove( foundFav );
        }

        public async Task<int> GetFavouritesCount( int recipeId )
        {
            return await _dbSet
                .Where( r => r.RecipeId == recipeId )
                .CountAsync();
        }

        public async Task<Favourite> GetFavouriteByAttributes( int userId, int recipeId )
        {
            return await _dbSet
                .Where( r => r.RecipeId == recipeId && r.UserId == userId )
                .FirstOrDefaultAsync();
        }
    }
}
