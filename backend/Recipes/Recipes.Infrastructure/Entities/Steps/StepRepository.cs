using Microsoft.EntityFrameworkCore;
using Recipes.Domain.Entities;

namespace Recipes.Infrastructure.Entities.Steps
{
    public class StepRepository : IStepRepository
    {
        private readonly DbContext _context;
        private readonly DbSet<Step> _steps;

        public StepRepository( DbContext context )
        {
            _context = context;
            _steps = _context.Set<Step>();
        }

        public async Task AddAsync( Step step )
        {
            await _steps.AddAsync( step );
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync( Step step )
        {
            _steps.Update( step );
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync( int id )
        {
            var step = await _steps.FindAsync( id );
            if ( step != null )
            {
                _steps.Remove( step );
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IReadOnlyList<Step>> GetByRecipeIdAsync( int recipeId )
        {
            return await _steps
                .Where( s => s.RecipeId == recipeId )
                .ToListAsync();
        }

        public async Task<Step> GetByStepNumberAsync( int recipeId, int stepNumber )
        {
            return await _steps
                .Where( s => s.RecipeId == recipeId && s.StepNumber == stepNumber )
                .FirstOrDefaultAsync();
        }
    }
}
