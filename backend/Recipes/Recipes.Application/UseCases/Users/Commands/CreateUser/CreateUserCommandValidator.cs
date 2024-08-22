using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Users.Commands.CreateUser;

public class CreateUserCommandValidator(
    IUserRepository userRepository )
    : IAsyncValidator<CreateUserCommand>
{
    public async Task<Result> ValidateAsync( CreateUserCommand command )
    {
        if ( string.IsNullOrEmpty( command.Login ) )
        {
            return Result.FromError( "Логин не может быть пустым" );
        }

        if ( await userRepository.ContainsAsync( user => user.Login == command.Login ) )
        {
            return Result.FromError( "Пользователь с таким логином уже существует" );
        }

        return Result.Success;
    }
}