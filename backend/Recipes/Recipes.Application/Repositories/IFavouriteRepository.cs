using Recipes.Application.Repositories.BasicRepositories;
using Recipes.Domain.Entities;

namespace Recipes.Application.Repositories;

public interface IFavouriteRepository :
    IAddEntityRepository<Favourite>,
    IDeleteEntityRepository<Favourite>,
    ISearchRepository<Favourite>
{
    Task<Favourite> GetFavouriteByAttributes( int recipeId, int userId );
}