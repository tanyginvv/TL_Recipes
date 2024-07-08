using Microsoft.EntityFrameworkCore;
using Recipes.Domain.Entities;
using Recipes.Infrastructure.Context;
using Recipes.Infrastructure.Entities;

namespace Recipes.Infrastructure.Repositories
{
    public class IngredientRepository : BaseRepository<Ingredient>, IIngredientRepository
    {
        private readonly RecipesDbContext _context;

        public IngredientRepository( RecipesDbContext context ) : base( context )
        {
            _context = context;
        }

        public async Task<IEnumerable<Ingredient>> GetAllIngredientsAsync()
        {
            return await _context.Ingredients.ToListAsync();
        }

        public async Task<IEnumerable<Ingredient>> GetIngredientByIdAsync( int id )
        {
            var ingredient = await _context.Ingredients
                .Where( i => i.Id == id )
                .ToListAsync();
            return ingredient;
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

        public async Task DeleteIngredientAsync( int id )
        {
            var ingredient = await _context.Ingredients.FindAsync( id );
            if ( ingredient != null )
            {
                _context.Ingredients.Remove( ingredient );
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Ingredient>> GetByRecipeIdAsync( int recipeId )
        {
            return await _context.Ingredients
                .Where( i => i.RecipeId == recipeId )
                .ToListAsync();
        }
    }
}