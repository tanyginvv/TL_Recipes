using Microsoft.EntityFrameworkCore;
using Recipes.Application.Repositories;
using Recipes.Domain.Entities;
using Recipes.Infrastructure.Context;

namespace Recipes.Infrastructure.Entities.Tags
{
    public class TagRepository : BaseRepository<Tag>, ITagRepository
    {
        private readonly RecipesDbContext _context;

        public TagRepository( RecipesDbContext context ) : base( context )
        {
            _context = context;
        }

        public async Task AddAsync( Tag tag )
        {
            await _context.Tags.AddAsync( tag );
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync( Tag tag )
        {
            _context.Tags.Update( tag );
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync( int id )
        {
            var tag = await _context.Tags.FindAsync( id );
            if ( tag != null )
            {
                _context.Tags.Remove( tag );
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IReadOnlyList<Tag>> GetByRecipeIdAsync( int recipeId )
        {
            return await _context.Tags
                .Where( t => t.Recipes.Any( r => r.Id == recipeId ) )
                .ToListAsync();
        }

        public async Task<Tag> GetByNameAsync( string name )
        {
            return await _context.Tags
                .FirstOrDefaultAsync( t => t.Name == name );
        }
    }
}
