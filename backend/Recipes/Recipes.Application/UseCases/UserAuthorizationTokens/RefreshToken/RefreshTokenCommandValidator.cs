
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.UserAuthorizationTokens.RefreshToken;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Application.UserAuthorizationTokens.Commands.RefreshToken;

public class RefreshTokenCommandValidator(
    IUserAuthorizationTokenRepository userAuthorizationTokenRepository ) 
    : IAsyncValidator<RefreshTokenCommand>
{
    public async Task<Result> ValidateAsync( RefreshTokenCommand command )
    {
        UserAuthorizationToken token = await userAuthorizationTokenRepository.GetByRefreshTokenAsync( command.RefreshToken );

        if ( token is null )
        {
            return Result.FromError( "Требуется авторизация" );
        }

        if ( DateTime.Now > token.ExpiryDate )
        {
            return Result.FromError( "Срок действия токена истек" );
        }

        return Result.FromSuccess();
    }
}