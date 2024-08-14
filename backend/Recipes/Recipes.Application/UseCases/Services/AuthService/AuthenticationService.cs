using Recipes.Application.Interfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Tokens.CreateToken;
using Recipes.Application.Tokens;
using Recipes.Domain.Entities;
using Recipes.Application.Results;
using Recipes.Application.PasswordHasher;

namespace Recipes.Application.UseCases.Services.AuthService;

public class AuthenticationService(
    IUserAuthorizationTokenRepository userAuthorizationTokenRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ITokenConfiguration tokenConfiguration,
    ITokenCreator tokenCreator,
    IPasswordHasher passwordHasher )
    : IAuthenticationService
{
    public async Task<Result<AuthenticateUserDto>> AuthenticateUserAsync( string login, string password )
    {
        User user = await userRepository.GetByLoginAsync( login );
        if ( user is null )
        {
            return Result<AuthenticateUserDto>.FromError( "Неверное имя пользователя или пароль" );
        }

        if ( !passwordHasher.VerifyPassword( password, user.PasswordHash ) )
        {
            return Result<AuthenticateUserDto>.FromError( "Введен неверный пароль" );
        }

        UserAuthorizationToken token = await userAuthorizationTokenRepository.GetByUserIdAsync( user.Id );
        if ( token is not null )
        {
            await userAuthorizationTokenRepository.Delete( token );
        }

        string accessToken = tokenCreator.GenerateAccessToken( user.Id );
        string refreshToken = tokenCreator.GenerateRefreshToken();

        DateTime refreshTokenExpiryDate = DateTime.Now.AddDays( tokenConfiguration.GetRefreshTokenValidityInDays() );

        UserAuthorizationToken newToken = new UserAuthorizationToken(
            user.Id,
            refreshToken,
            refreshTokenExpiryDate );
        await userAuthorizationTokenRepository.AddAsync( newToken );

        await unitOfWork.CommitAsync();

        AuthenticateUserDto authenticateUserCommandResult = new AuthenticateUserDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
        return Result<AuthenticateUserDto>.FromSuccess( authenticateUserCommandResult );
    }
}