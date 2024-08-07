using Recipes.Application.Repositories.BasicRepositories;
using Recipes.Domain.Entities;

namespace Recipes.Application.Repositories
{
    public interface ILikeRepository :
        IAddedRepository<Like>,
        IRemovableRepository<Like>,
        ISearchRepository<Like>
    {
        Task<Like> GetLikeByAttributes( int userId, int recipeId );
        Task<int> GetLikesCount( int recipeId );
    }
}