using Recipes.Application.PasswordHasher;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Users.Commands.UpdateUser;

public class UpdateUserCommandValidator(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher )
    : IAsyncValidator<UpdateUserCommand>
{
    public async Task<Result> ValidateAsync( UpdateUserCommand command )
    {
        User user = await userRepository.GetByIdAsync( command.Id );
        if ( user is null )
        {
            return Result.FromError( "Пользователь не найден." );
        }

        if ( !string.IsNullOrEmpty( command.OldPassword ) && !passwordHasher.VerifyPassword( command.OldPassword, user.PasswordHash ) )
        {
            return Result.FromError( "Введеный пароль неверный" );
        }

        if ( !string.IsNullOrEmpty( command.Login ) )
        {
            User existingUserWithNewLogin = await userRepository.GetByLoginAsync( command.Login );
            if ( existingUserWithNewLogin is not null && existingUserWithNewLogin.Id != user.Id )
            {
                return Result.FromError( "Новый логин уже используется другим пользователем." );
            }
        }

        return Result.FromSuccess();
    }
}