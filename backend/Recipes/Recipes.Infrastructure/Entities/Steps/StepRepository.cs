using Microsoft.EntityFrameworkCore;
using Recipes.Application.Repositories;
using Recipes.Domain.Entities;
using Recipes.Infrastructure.Context;

namespace Recipes.Infrastructure.Entities.Steps
{
    public class StepRepository : BaseRepository<Step>, IStepRepository
    {
        public StepRepository( RecipesDbContext context ) : base( context )
        {
        }

        public override async Task AddAsync( Step step )
        {
            await base.AddAsync( step );
        }

        public async Task Delete( Step step )
        {
            Step st = await GetByIdAsync( step.Id );
            if ( st is not null )
            {
                base.Remove( step );
            }
        }

        public async Task<IReadOnlyList<Step>> GetByRecipeIdAsync( int recipeId )
        {
            return await _dbSet
                .Where( s => s.RecipeId == recipeId )
                .ToListAsync();
        }

        public async Task<Step> GetByStepIdAsync( int stepId )
        {
            return await _dbSet
                .Where( s => s.Id == stepId )
                .FirstOrDefaultAsync();
        }
    }
}
