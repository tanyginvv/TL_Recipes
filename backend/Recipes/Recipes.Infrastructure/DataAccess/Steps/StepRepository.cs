using Microsoft.EntityFrameworkCore;
using Recipes.Application.Repositories;
using Recipes.Domain.Entities;

namespace Recipes.Infrastructure.DataAccess.Steps;

public class StepRepository( RecipesDbContext context ) : BaseRepository<Step>( context ), IStepRepository
{
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