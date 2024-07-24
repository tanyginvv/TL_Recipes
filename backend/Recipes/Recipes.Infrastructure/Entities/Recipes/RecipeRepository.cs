using Microsoft.EntityFrameworkCore;
using Recipes.Application.Paginator;
using Recipes.Application.Repositories;
using Recipes.Domain.Entities;
using Recipes.Infrastructure.Context;
using Recipes.Infrastructure.Entities.Specification;

namespace Recipes.Infrastructure.Entities.Recipes
{
    public class RecipeRepository : BaseRepository<Recipe>, IRecipeRepository
    {
        public RecipeRepository( RecipesDbContext context ) : base( context )
        {
        }

        public override async Task AddAsync( Recipe recipe )
        {
            await base.AddAsync( recipe );
        }

        public async Task Delete( Recipe recipe )
        {
            Recipe rec = await GetByIdAsync( recipe.Id );
            if ( rec is not null )
            {
                base.Remove( rec );
            }
        }

        public async Task<IReadOnlyList<Recipe>> GetAllAsync( PaginationFilter paginationFilter )
        {
            RecipeSpecification spec = new RecipeSpecification( null, paginationFilter );
            return await spec.Apply( _dbSet ).ToListAsync();
        }

        public async Task<IReadOnlyList<Recipe>> GetFilteredRecipesAsync( IEnumerable<string> searchTerms, PaginationFilter paginationFilter )
        {
            RecipeSpecification spec = new RecipeSpecification( searchTerms, paginationFilter );
            return await spec.Apply( _dbSet ).ToListAsync();
        }

        public override async Task<Recipe> GetByIdAsync( int id )
        {
            return await _dbSet
                .Include( r => r.Steps )
                .Include( r => r.Ingredients )
                .Include( r => r.Tags )
                .FirstOrDefaultAsync( r => r.Id == id );
        }
    }
}
