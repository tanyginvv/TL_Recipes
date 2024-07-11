using Microsoft.EntityFrameworkCore;
using Recipes.Domain.Entities;
using Recipes.Infrastructure.Context;

namespace Recipes.Infrastructure.Entities.Steps
{
    public class StepRepository : BaseRepository<Step>, IStepRepository
    {
        private readonly RecipesDbContext _context;

        public StepRepository( RecipesDbContext context ) : base( context )
        {
            _context = context;
        }

        public async Task AddAsync( Step step )
        {
            await _context.Steps.AddAsync( step );
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync( Step step )
        {
            _context.Steps.Update( step );
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync( int id )
        {
            var step = await _context.Steps.FindAsync( id );
            if ( step != null )
            {
                _context.Steps.Remove( step );
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IReadOnlyList<Step>> GetByRecipeIdAsync( int recipeId )
        {
            return await _context.Steps
                .Where( s => s.RecipeId == recipeId )
                .ToListAsync();
        }

        public async Task<Step> GetByStepNumberAsync( int recipeId, int stepNumber )
        {
            return await _context.Steps
                .Where( s => s.RecipeId == recipeId && s.StepNumber == stepNumber )
                .FirstOrDefaultAsync();
        }
    }
}
