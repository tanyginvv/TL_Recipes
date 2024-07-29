using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Users.Commands.DeleteUser;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Application.Users.Commands.DeleteUser
{
    public class DeleteUserCommandValidator( IUserRepository userRepository )
        : IAsyncValidator<DeleteUserCommand>
    {
        public async Task<Result> ValidateAsync( DeleteUserCommand command )
        {
            if ( !await userRepository.ContainsAsync( user => user.Id == command.userId ) )
            {
                return Result.FromError( "Пользователя с таким id не cуществует" );
            }

            User user = await userRepository.GetByIdAsync( command.userId );
            if ( user.PasswordHash != command.PasswordHash )
            {
                return Result.FromError( "Введенный пароль не совпадает с текущим" );
            }

            return Result.FromSuccess();
        }
    }
}