using Microsoft.EntityFrameworkCore;
using Recipes.Application.Filters;
using Recipes.Application.Interfaces;
using Recipes.Application.Repositories;
using Recipes.Domain.Entities;
using Recipes.Infrastructure.Context;

namespace Recipes.Infrastructure.Entities.Recipes
{
    public class RecipeRepository : BaseRepository<Recipe>, IRecipeRepository
    {
        public RecipeRepository( RecipesDbContext context ) : base( context )
        {
        }

        public async Task AddAsync( Recipe recipe )
        {
            await base.AddAsync( recipe );
        }

        public async Task Delete( Recipe recipe )
        {
            Recipe rec = await GetByIdAsync( recipe.Id );
            if ( rec is not null )
            {
                base.Remove( rec );
            }
        }

        public async Task<List<Recipe>> GetRecipesAsync( IEnumerable<IFilter<Recipe>> filters )
        {
            IQueryable<Recipe> query = _dbSet
                .AsQueryable().ApplyFilters( filters );

            List<Recipe> recipes = await query.ToListAsync();

            foreach ( Recipe recipe in recipes )
            {
                await _context.Entry( recipe ).Collection( r => r.Tags ).LoadAsync();
            }

            return recipes;
        }

        public override async Task<Recipe> GetByIdAsync( int id )
        {
            return await _dbSet
                .Include( r => r.Steps )
                .Include( r => r.Ingredients )
                .Include( r => r.Tags )
                .FirstOrDefaultAsync( r => r.Id == id );
        }
    }
}
