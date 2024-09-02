using Recipes.Application.Results;
using Recipes.Application.UseCases.Services;

namespace Recipes.Application.Interfaces;
public interface IAuthTokenService
{
    public Task<Result<TokenDto>> GenerateTokensAsync( int userId );
}