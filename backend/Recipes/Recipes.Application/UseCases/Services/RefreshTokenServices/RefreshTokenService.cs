using Recipes.Application.Interfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Services.RefreshTokenServices;
public class RefreshTokenService(
    IUserAuthTokenRepository userAuthTokenRepository,
    IAuthTokenService authTokenService )
    : IRefreshTokenService
{
    public async Task<Result<TokenDto>> RefreshTokenAsync( string refreshToken )
    {
        UserAuthToken userAuthToken = await userAuthTokenRepository.GetByRefreshTokenAsync( refreshToken );
        if ( userAuthToken is null || userAuthToken.ExpiryDate < DateTime.UtcNow )
        {
            return Result<TokenDto>.FromError( "Недействительный или истекший токен" );
        }

        return await authTokenService.GenerateTokensAsync( userAuthToken.UserId );
    }
}