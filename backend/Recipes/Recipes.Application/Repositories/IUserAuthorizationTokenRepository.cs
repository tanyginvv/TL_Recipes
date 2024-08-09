using Recipes.Application.Repositories.BasicRepositories;
using Recipes.Domain.Entities;

namespace Recipes.Application.Repositories;

public interface IUserAuthorizationTokenRepository :
    IAddedRepository<UserAuthorizationToken>,
    IRemovableRepository<UserAuthorizationToken>,
    ISearchRepository<UserAuthorizationToken>
{
    Task<UserAuthorizationToken> GetByUserIdAsync( long userId );
    Task<UserAuthorizationToken> GetByRefreshTokenAsync( string refreshToken );
}
