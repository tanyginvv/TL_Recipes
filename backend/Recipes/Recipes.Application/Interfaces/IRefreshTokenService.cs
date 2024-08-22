using Recipes.Application.Results;
using Recipes.Application.UseCases.Services;

namespace Recipes.Application.Interfaces;

public interface IRefreshTokenService
{
    public Task<Result<TokenDto>> RefreshTokenAsync( string refreshToken );
}