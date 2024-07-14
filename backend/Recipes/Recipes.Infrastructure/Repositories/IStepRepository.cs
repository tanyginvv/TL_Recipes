using Recipes.Domain.Entities;

namespace Recipes.Infrastructure.Repositories
{
    public interface IStepRepository
    {
        Task<IReadOnlyList<Step>> GetByRecipeIdAsync( int recipeId );
        Task<Step> GetByStepIdAsync( int stepId );
        Task AddAsync( Step step );
        Task UpdateAsync( Step step );
        Task DeleteAsync( int id );
    }
}
