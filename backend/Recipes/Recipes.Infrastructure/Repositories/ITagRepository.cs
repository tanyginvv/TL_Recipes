using Recipes.Domain.Entities;

namespace Recipes.Infrastructure.Repositories
{
    public interface ITagRepository
    {
        Task<IReadOnlyList<Tag>> GetByRecipeIdAsync( int recipeId );
        Task AddAsync( Tag tag );
        Task UpdateAsync( Tag tag );
        Task DeleteAsync( int id );
        Task<Tag> GetByNameAsync( string name );
    }
}