using Recipes.Application.Repositories.BasicRepositories;
using Recipes.Domain.Entities;

namespace Recipes.Application.Repositories;

public interface IUserAuthorizationTokenRepository :
    IAddEntityRepository<UserAuthorizationToken>,
    IDeleteEntityRepository<UserAuthorizationToken>,
    ISearchRepository<UserAuthorizationToken>
{
    Task<UserAuthorizationToken> GetByUserIdAsync( int userId );
    Task<UserAuthorizationToken> GetByRefreshTokenAsync( string refreshToken );
}