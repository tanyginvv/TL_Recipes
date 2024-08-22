using Microsoft.EntityFrameworkCore;
using Recipes.Application.Filters;
using Recipes.Application.Interfaces;
using Recipes.Application.Repositories;
using Recipes.Domain.Entities;
using Recipes.Infrastructure.Context;

namespace Recipes.Infrastructure.Entities.Recipes;

public class RecipeRepository( RecipesDbContext context ) : BaseRepository<Recipe>(context), IRecipeRepository
{
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
        return await _dbSet
           .ApplyFilters( filters )
           .Include( r => r.Tags )
           .Include( r => r.Author )  
           .ToListAsync();

    }

    public override async Task<Recipe> GetByIdAsync( int id )
    {
        return await _dbSet
            .Include( r => r.Steps )
            .Include( r => r.Ingredients )
            .Include( r => r.Tags )
            .Include( r => r.Author )
            .FirstOrDefaultAsync( r => r.Id == id );
    }
}