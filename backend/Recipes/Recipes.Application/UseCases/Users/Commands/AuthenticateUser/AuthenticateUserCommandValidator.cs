using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Users.Commands.AuthenticatePassword;
using Recipes.Application.Validation;

namespace Application.UserAuthorizationTokens.Commands.AuthenticateUser
{
    public class AuthenticateUserCommandValidator( IUserRepository userRepository )
        : IAsyncValidator<AuthenticateUserCommand>
    {
        public async Task<Result> ValidateAsync( AuthenticateUserCommand command )
        {
            if ( string.IsNullOrEmpty( command.Login ) )
            {
                return Result.FromError( "Логин не может быть пустым" );
            }

            if ( !await userRepository.ContainsAsync( user => user.Login == command.Login && user.PasswordHash == command.PasswordHash ) )
            {
                return Result.FromError( "Неверное имя пользователя или пароль" );
            }

            return Result.FromSuccess();
        }
    }
}