using Application.Repositories;
using Microsoft.EntityFrameworkCore;
using Recipes.Domain.Entities;
using Recipes.Infrastructure.Context;

namespace Recipes.Infrastructure.Entities.Recipes
{
    public class RecipeRepository : BaseRepository<Recipe>, IRecipeRepository
    {
        private readonly RecipesDbContext _context;

        public RecipeRepository( RecipesDbContext context ) : base( context )
        {
            _context = context;
        }

        public async Task AddAsync( Recipe recipe )
        {
            await _context.Set<Recipe>().AddAsync( recipe );
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync( int id )
        {
            var recipe = await GetByIdAsync( id );
            if ( recipe != null )
            {
                _context.Set<Recipe>().Remove( recipe );
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IReadOnlyList<Recipe>> GetAllAsync()
        {
            return await _context.Set<Recipe>().ToListAsync();
        }

        public async Task<Recipe> GetByIdAsync( int id )
        {
            return await _context.Set<Recipe>().FindAsync( id );
        }

        public async Task UpdateAsync( Recipe recipe )
        {
            _context.Set<Recipe>().Update( recipe );
            await _context.SaveChangesAsync();
        }
    }
}
