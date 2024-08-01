using Recipes.Application.Repositories.BasicRepositories;
using Recipes.Domain.Entities;

namespace Recipes.Application.Repositories
{
    public interface ITagRepository :
        IAddedRepository<Tag>
    {
        Task<IReadOnlyList<Tag>> GetByRecipeIdAsync( int recipeId );
        Task<IReadOnlyList<Tag>> GetTagsForSearchAsync( int count );
        Task<Tag> GetByNameAsync( string name );
    }
}