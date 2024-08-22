using Recipes.Application.Repositories.BasicRepositories;
using Recipes.Domain.Entities;

namespace Recipes.Application.Repositories;

public interface IUserAuthTokenRepository :
    IAddEntityRepository<UserAuthToken>,
    IDeleteEntityRepository<UserAuthToken>,
    ISearchRepository<UserAuthToken>
{
    Task<UserAuthToken> GetByUserIdAsync( int userId );
    Task<UserAuthToken> GetByRefreshTokenAsync( string refreshToken );
}