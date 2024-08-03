using Microsoft.EntityFrameworkCore;
using Recipes.Application.Repositories;
using Recipes.Domain.Entities;
using Recipes.Infrastructure.Context;

namespace Recipes.Infrastructure.Entities.Tags
{
    public class TagRepository : BaseRepository<Tag>, ITagRepository
    {
        public TagRepository( RecipesDbContext context ) : base( context )
        {
        }

        public async Task<IReadOnlyList<Tag>> GetByRecipeIdAsync( int recipeId )
        {
            return await _dbSet
                .Where( t => t.Recipes.Any( r => r.Id == recipeId ) )
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Tag>> GetTagsForSearchAsync( int count )
        {
            List<Tag> allTags = await _dbSet.ToListAsync();
            List<Tag> randomTags = allTags.OrderBy( _ => Guid.NewGuid() ).Take( count ).ToList();
            return randomTags;
        }

        public async Task<Tag> GetByNameAsync( string name )
        {
            return await _dbSet.FirstOrDefaultAsync( t => t.Name == name );
        }

        public override async Task AddAsync( Tag tag )
        {
            await base.AddAsync( tag );
        }
    }
}
