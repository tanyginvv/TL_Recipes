using Recipes.Application.Repositories.BasicRepositories;
using Recipes.Domain.Entities;

namespace Recipes.Application.Repositories;

public interface IStepRepository :
    IAddEntityRepository<Step>,
    IDeleteEntityRepository<Step>
{
    Task<IReadOnlyList<Step>> GetByRecipeIdAsync( int recipeId );
    Task<Step> GetByStepIdAsync( int stepId );
}