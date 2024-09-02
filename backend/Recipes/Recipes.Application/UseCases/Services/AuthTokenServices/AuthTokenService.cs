using Recipes.Application.Interfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Tokens.CreateToken;
using Recipes.Domain.Entities;
using Recipes.Application.Tokens;

namespace Recipes.Application.UseCases.Services.AuthTokenServices;

public class AuthTokenService(
    IUserAuthTokenRepository userAuthTokenRepository,
    ITokenCreator tokenCreator,
    ITokenConfiguration tokenConfiguration,
    IUnitOfWork unitOfWork )
    : IAuthTokenService
{
    public async Task<Result<TokenDto>> GenerateTokensAsync( int userId )
    {
        UserAuthToken existingToken = await userAuthTokenRepository.GetByUserIdAsync( userId );
        if ( existingToken is not null )
        {
            await userAuthTokenRepository.Delete( existingToken );
        }

        string accessToken = tokenCreator.GenerateAccessToken( userId );
        string refreshToken = tokenCreator.GenerateRefreshToken();
        DateTime refreshTokenExpiryDate = DateTime.UtcNow.AddDays( tokenConfiguration.GetRefreshTokenValidityInDays() );

        UserAuthToken newToken = new UserAuthToken( userId, refreshToken, refreshTokenExpiryDate );
        await userAuthTokenRepository.AddAsync( newToken );

        await unitOfWork.CommitAsync();

        TokenDto result = new TokenDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };

        return Result<TokenDto>.FromSuccess( result );
    }
}