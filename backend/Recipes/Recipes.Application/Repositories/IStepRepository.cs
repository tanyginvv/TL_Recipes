using Recipes.Domain.Entities;

namespace Recipes.Application.Repositories
{
    public interface IStepRepository :
        IAddedRepository<Step>,
        IRemovableRepository<Step>
    {
        Task<IReadOnlyList<Step>> GetByRecipeIdAsync( int recipeId );
        Task<Step> GetByStepIdAsync( int stepId );
    }
}
