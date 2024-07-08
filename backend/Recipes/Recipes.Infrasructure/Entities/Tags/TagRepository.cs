using Azure;
using Microsoft.EntityFrameworkCore;
using Recipes.Domain.Entities;
using Recipes.Infrastructure.Context;

namespace Recipes.Infrastructure.Entities.Tags
{
    public class TagRepository : ITagRepository
    {
        private readonly RecipesDbContext _context;

        public TagRepository( RecipesDbContext context )
        {
            _context = context;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task<Tag> GetByIdAsync( int id )
        {
            return await _context.Tags.FindAsync( id );
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

        public async Task<IEnumerable<Tag>> GetByRecipeIdAsync( int recipeId )
        {
            return await _context.Tags
                .Where( t => t.Id == recipeId )//неправильно пока
                .ToListAsync();
        }
    }
}
