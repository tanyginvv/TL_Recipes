using Recipes.Application.Interfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Results;
using Recipes.Application.PasswordHasher;

namespace Recipes.Application.UseCases.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler(
    IUserRepository userRepository,
    IAsyncValidator<UpdateUserCommand> validator,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher )
    : ICommandHandler<UpdateUserCommand>
{
    public async Task<Result> HandleAsync( UpdateUserCommand command )
    {
        Result validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsSuccess )
        {
            return validationResult;
        }

        User user = await userRepository.GetByIdAsync( command.Id );
        if ( user is null )
        {
            return Result.FromError( "Пользователь не найден." );
        }
        user.Name = command.Name;
        user.Description = command.Description;

        if ( !string.IsNullOrEmpty( command.Login ) )
        {
            user.Login = command.Login;
        }

        if ( !string.IsNullOrEmpty( command.OldPassword ) && !string.IsNullOrEmpty( command.NewPassword ) )
        {
            string hashedPassword = passwordHasher.GeneratePassword( command.NewPassword );
            user.PasswordHash = hashedPassword;
        }

        await unitOfWork.CommitAsync();

        return Result.FromSuccess();
    }
}