using Microsoft.EntityFrameworkCore;
using Recipes.Application.Repositories;
using Recipes.Domain.Entities;
using Recipes.Infrastructure.DataAccess;

namespace Recipes.Infrastructure.DataAccess.Tags;

public class TagRepository( RecipesDbContext context ) : BaseRepository<Tag>( context ), ITagRepository
{
    public async Task<IReadOnlyList<Tag>> GetByRecipeIdAsync( int recipeId )
    {
        return await _dbSet
            .Where( t => t.Recipes.Any( r => r.Id == recipeId ) )
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Tag>> GetTagsForSearchAsync( int count )
    {
        return await _dbSet
            .OrderBy( _ => Guid.NewGuid() )
            .Take( count )
            .ToListAsync();
    }

    public async Task<Tag> GetByNameAsync( string name )
    {
        return await _dbSet.FirstOrDefaultAsync( t => t.Name == name );
    }
}