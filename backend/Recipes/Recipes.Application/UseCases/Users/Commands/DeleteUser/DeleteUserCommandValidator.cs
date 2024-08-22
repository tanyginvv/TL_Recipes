using Recipes.Application.PasswordHasher;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Users.Commands.DeleteUser;

public class DeleteUserCommandValidator(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher )
        : IAsyncValidator<DeleteUserCommand>
{
    public async Task<Result> ValidateAsync( DeleteUserCommand command )
    {
        if ( !await userRepository.ContainsAsync( user => user.Id == command.UserId ) )
        {
            return Result.FromError( "Пользователя с таким id не cуществует" );
        }

        User user = await userRepository.GetByIdAsync( command.UserId );
        if ( !passwordHasher.VerifyPassword( command.PasswordHash, user.PasswordHash ) )
        {
            return Result.FromError( "Введеный пароль неверный" );
        }

        return Result.FromSuccess();
    }
}