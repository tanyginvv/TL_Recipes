using Microsoft.EntityFrameworkCore;
using Recipes.Application.Repositories;
using Recipes.Domain.Entities;
using Recipes.Infrastructure.Context;

namespace Recipes.Infrastructure.Entities.Ingredients
{
    public class IngredientRepository : BaseRepository<Ingredient>, IIngredientRepository
    {
        public IngredientRepository( RecipesDbContext context ) : base( context )
        {
        }

        public override async Task AddAsync( Ingredient ingredient )
        {
            await base.AddAsync( ingredient );
        }

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
}
