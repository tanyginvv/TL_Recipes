using Recipes.Domain.Entities;

namespace Recipes.Application.Repositories
{
    public interface ITagRepository
    {
        Task<IReadOnlyList<Tag>> GetByRecipeIdAsync( int recipeId );
        Task<IReadOnlyList<Tag>> GetRandomTagsAsync( int count );
        Task AddAsync( Tag tag );
        Task UpdateAsync( Tag tag );
        Task DeleteAsync( int id );
        Task<Tag> GetByNameAsync( string name );
    }
}