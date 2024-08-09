using Recipes.Application.Repositories.BasicRepositories;
using Recipes.Domain.Entities;

namespace Recipes.Application.Repositories;

public interface IFavouriteRepository :
    IAddedRepository<Favourite>,
    IRemovableRepository<Favourite>,
    ISearchRepository<Favourite>
{
    Task<Favourite> GetFavouriteByAttributes( int recipeId, int userId );
    Task<int> GetFavouritesCount( int recipeId );
}