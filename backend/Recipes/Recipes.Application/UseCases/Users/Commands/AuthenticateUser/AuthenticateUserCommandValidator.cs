using Recipes.Application.PasswordHasher;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Users.Commands.AuthenticatePassword;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Application.UserAuthorizationTokens.Commands.AuthenticateUser;

public class AuthenticateUserCommandValidator(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher )
    : IAsyncValidator<AuthenticateUserCommand>
{
    public async Task<Result> ValidateAsync( AuthenticateUserCommand command )
    {
        if ( string.IsNullOrEmpty( command.Login ) )
        {
            return Result.FromError( "Логин не может быть пустым" );
        }

        User user = await userRepository.GetByLoginAsync( command.Login );
        if ( user is null )
        {
            return Result.FromError( "Неверное имя пользователя или пароль" );
        }

        if ( !passwordHasher.VerifyPassword( command.PasswordHash, user.PasswordHash ) )
        {
            return Result.FromError( "Введен неверный пароль" );
        }

        return Result.FromSuccess();
    }
}