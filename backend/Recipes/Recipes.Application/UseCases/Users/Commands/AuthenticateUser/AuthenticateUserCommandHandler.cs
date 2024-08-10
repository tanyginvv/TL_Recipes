using Recipes.Application.Interfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Tokens.CreateToken;
using Recipes.Application.Tokens;
using Recipes.Application.UseCases.Users.Commands.AuthenticatePassword;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.UseCases.Users.Dto;
using Recipes.Application.Results;

namespace Application.UserAuthorizationTokens.Commands.AuthenticateUser;

public class AuthenticateUserCommandHandler(
    IUserAuthorizationTokenRepository userAuthorizationTokenRepository,
    IUserRepository userRepository,
    IAsyncValidator<AuthenticateUserCommand> validator,
    IUnitOfWork unitOfWork,
    ITokenConfiguration tokenConfiguration ) 
    : ICommandHandlerWithResult<AuthenticateUserCommand, AuthenticateUserCommandDto>
{
    private TokenCreator tokenCreator => new TokenCreator( tokenConfiguration );

    public async Task<Result<AuthenticateUserCommandDto>> HandleAsync( AuthenticateUserCommand command )
    {
        Result validationResult = await validator.ValidateAsync( command );

        if ( !validationResult.IsSuccess )
        {
            return Result<AuthenticateUserCommandDto>.FromError( validationResult.Error );
        }

        User user = await userRepository.GetByLoginAsync( command.Login );
        UserAuthorizationToken token = await userAuthorizationTokenRepository.GetByUserIdAsync( user.Id );
        if ( token is not null )
        {
            await userAuthorizationTokenRepository.Delete( token );
        }

        string accessToken = tokenCreator.GenerateAccessToken( user.Id );
        string refreshToken = TokenCreator.GenerateRefreshToken();

        DateTime refreshTokenExpiryDate = DateTime.Now.AddDays( tokenConfiguration.GetRefreshTokenValidityInDays() );

        UserAuthorizationToken newToken = new UserAuthorizationToken(
            user.Id,
            refreshToken,
            refreshTokenExpiryDate );
        await userAuthorizationTokenRepository.AddAsync( newToken );

        await unitOfWork.CommitAsync();

        AuthenticateUserCommandDto authenticateUserCommandResult = new AuthenticateUserCommandDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
        return Result<AuthenticateUserCommandDto>.FromSuccess( authenticateUserCommandResult );
    }
}