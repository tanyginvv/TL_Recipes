using Microsoft.EntityFrameworkCore;
using Recipes.Application.Repositories;
using Recipes.Domain.Entities;
using Recipes.Infrastructure.Context;

namespace Recipes.Infrastructure.Repositories
{
    public class IngredientRepository : BaseRepository<Ingredient>, IIngredientRepository
    {
        private readonly RecipesDbContext _context;

        public IngredientRepository( RecipesDbContext context ) : base( context )
        {
            _context = context;
        }

        public async Task AddIngredientAsync( Ingredient ingredient )
        {
            await _context.Ingredients.AddAsync( ingredient );
            await _context.SaveChangesAsync();
        }

        public async Task UpdateIngredientAsync( Ingredient ingredient )
        {
            _context.Ingredients.Update( ingredient );
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync( int id )
        {
            var ingredient = await _context.Ingredients.FindAsync( id );
            if ( ingredient != null )
            {
                _context.Ingredients.Remove( ingredient );
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IReadOnlyList<Ingredient>> GetByRecipeIdAsync( int recipeId )
        {
            return await _context.Ingredients
                .Where( i => i.RecipeId == recipeId )
                .ToListAsync();
        }

        public async Task<Ingredient> GetByIdAsync( int id )
        {
            return await _context.Ingredients.FindAsync( id );
        }
    }
}