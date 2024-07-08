using Recipes.Domain.Entities;

namespace Recipes.Infrastructure.Entities.Tags
{
    public interface ITagRepository
    {
        Task<Tag> GetByIdAsync( int id );
        Task<IEnumerable<Tag>> GetAllAsync();
        Task<IEnumerable<Tag>> GetByRecipeIdAsync( int recipeId );
        Task AddAsync( Tag tag );
        Task UpdateAsync( Tag tag );
        Task DeleteAsync( int id );
    }
}
