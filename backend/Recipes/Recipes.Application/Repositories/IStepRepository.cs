using Recipes.Domain.Entities;

namespace Recipes.Infrastructure.Entities.Steps
{
    public interface IStepRepository
    {
        Task<IEnumerable<Step>> GetByRecipeIdAsync( int recipeId );
        Task<Step> GetByStepNumberAsync( int recipeId, int stepNumber );
        Task AddAsync( Step step );
        Task UpdateAsync( Step step );
        Task DeleteAsync( int id );
    }
}
