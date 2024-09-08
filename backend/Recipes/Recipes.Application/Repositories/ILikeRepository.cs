using Recipes.Application.Repositories.BasicRepositories;
using Recipes.Domain.Entities;

namespace Recipes.Application.Repositories;

public interface ILikeRepository :
    IAddEntityRepository<Like>,
    IDeleteEntityRepository<Like>
{
    Task<Like> GetLikeByAttributes( int userId, int recipeId );
}