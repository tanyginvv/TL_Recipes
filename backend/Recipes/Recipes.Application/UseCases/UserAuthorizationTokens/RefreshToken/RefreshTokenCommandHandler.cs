using Recipes.Application.Interfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Tokens.CreateToken;
using Recipes.Application.Tokens;
using Recipes.Application.UseCases.UserAuthorizationTokens.RefreshToken;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;
using Recipes.Application.UseCases.UserAuthorizationTokens.Dto;
using Recipes.Application.Results;
using Recipes.Application.CQRSInterfaces;

namespace Application.UseCases.UserAuthorizationTokens;

public class RefreshTokenCommandHandler(
    IUserAuthorizationTokenRepository userAuthorizationTokenRepository,
    IAsyncValidator<RefreshTokenCommand> validator,
    IUnitOfWork unitOfWork,
    ITokenConfiguration tokenConfiguration,
    ITokenCreator tokenCreator )
    : ICommandHandlerWithResult<RefreshTokenCommand, RefreshTokenCommandDto>
{
    public async Task<Result<RefreshTokenCommandDto>> HandleAsync( RefreshTokenCommand command )
    {
        Result validationResult = await validator.ValidateAsync( command );

        if ( !validationResult.IsSuccess )
        {
            return Result<RefreshTokenCommandDto>.FromError( validationResult );
        }

        UserAuthorizationToken token = await userAuthorizationTokenRepository.GetByRefreshTokenAsync( command.RefreshToken );
        await userAuthorizationTokenRepository.Delete( token );

        string accessToken = tokenCreator.GenerateAccessToken( token.UserId );
        string refreshToken = tokenCreator.GenerateRefreshToken();
        DateTime refreshTokenExpiryDate = DateTime.Now.AddDays( tokenConfiguration.GetRefreshTokenValidityInDays() );

        UserAuthorizationToken newToken = new UserAuthorizationToken(
            token.UserId,
            refreshToken,
            refreshTokenExpiryDate );
        await userAuthorizationTokenRepository.AddAsync( newToken );

        await unitOfWork.CommitAsync();

        RefreshTokenCommandDto refreshTokenCommandResult = new RefreshTokenCommandDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };

        return Result<RefreshTokenCommandDto>.FromSuccess( refreshTokenCommandResult );
    }
}