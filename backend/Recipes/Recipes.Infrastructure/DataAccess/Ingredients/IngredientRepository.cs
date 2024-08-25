using Microsoft.EntityFrameworkCore;
using Recipes.Application.Repositories;
using Recipes.Domain.Entities;
using Recipes.Infrastructure.DataAccess;

namespace Recipes.Infrastructure.DataAccess.Ingredients;

public class IngredientRepository( RecipesDbContext context ) : BaseRepository<Ingredient>( context ), IIngredientRepository
{
    public async Task Delete( Ingredient ingredient )
    {
        Ingredient ingred = await GetByIdAsync( ingredient.Id );
        if ( ingredient is not null )
        {
            base.Remove( ingred );
        }
    }

    public async Task<IReadOnlyList<Ingredient>> GetByRecipeIdAsync( int recipeId )
    {
        return await _dbSet
            .Where( i => i.RecipeId == recipeId )
            .ToListAsync();
    }

    public override async Task<Ingredient> GetByIdAsync( int id )
    {
        return await _dbSet.FindAsync( id );
    }
}