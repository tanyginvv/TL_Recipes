using Azure;
using Microsoft.EntityFrameworkCore;
using Recipes.Domain.Entities;
using Recipes.Infrastructure.Context;

namespace Recipes.Infrastructure.Entities.Tags
{
    public class TagRepository : ITagRepository
    {
        private readonly RecipesDbContext _context;
        private readonly DbSet<Tag> _tags;

        public TagRepository( DbContext context )
        {
            _context = ( RecipesDbContext )context;
            _tags = _context.Set<Tag>();
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
            return await _tags
                .Where( t => t.Recipes.Any( r => r.Id == recipeId ) )
                .ToListAsync();
        }
    }
}
