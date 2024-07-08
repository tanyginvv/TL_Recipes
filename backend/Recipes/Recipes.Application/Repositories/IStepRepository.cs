using Recipes.Domain.Entities;

namespace Recipes.Infrastructure.Entities.Steps
{
    public interface IStepRepository
    {
        Task<Step> GetByIdAsync( int id );
        Task<IEnumerable<Step>> GetAllAsync();
        Task<IEnumerable<Step>> GetByRecipeIdAsync( int recipeId );
        Task AddAsync( Step step );
        Task UpdateAsync( Step step );
        Task DeleteAsync( int id );
    }
}
