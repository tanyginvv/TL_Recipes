using Recipes.Domain.Entities;

namespace Recipes.Application.Repositories
{
    public interface ITagRepository
    {
        Task<IReadOnlyList<Tag>> GetByRecipeIdAsync( int recipeId );
        Task<IReadOnlyList<Tag>> GetTagsForSearchAsync( int count );
        Task<Tag> GetByNameAsync( string name );
        Task AddAsync( Tag tag );
        Task DeleteAsync( int id );
    }
}