using Microsoft.EntityFrameworkCore;
using Recipes.Application.Repositories;
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
            return await _context.Set<Recipe>()
                         .Include( r => r.Steps )
                         .Include( r => r.Ingredients )
                         .Include( r => r.Tags )
                         .ToListAsync();
        }

        public async Task<IReadOnlyList<Recipe>> GetFilteredRecipesAsync( IEnumerable<string> searchTerms )
        {
            var normalizedSearchTerms = searchTerms.Select( term => term.ToLower() ).ToList();

            return await _context.Set<Recipe>()
                .Include( r => r.Steps )
                .Include( r => r.Ingredients )
                .Include( r => r.Tags )
                .Where( r =>
                    normalizedSearchTerms.Any( term =>
                        r.Tags.Any( tag => tag.Name.ToLower().Equals( term ) ) ||
                        r.Name.ToLower().Contains( term )
                    )
                )
                .ToListAsync();
        }


        public async Task<Recipe> GetByIdAsync( int id )
        {
            return await _context.Set<Recipe>()
                .Include( r => r.Steps )
                .Include( r => r.Ingredients )
                .Include( r => r.Tags )
                .Where( r => r.Id == id )
                .SingleOrDefaultAsync();
        }

        public async Task UpdateAsync( Recipe recipe )
        {
            _context.Set<Recipe>().Update( recipe );
            await _context.SaveChangesAsync();
        }
    }
}