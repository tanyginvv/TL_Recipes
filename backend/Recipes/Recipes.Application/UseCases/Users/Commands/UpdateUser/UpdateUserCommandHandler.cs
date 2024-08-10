using Recipes.Application.Interfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Users.Commands.UpdateUser;
using Recipes.Application.PasswordHasher;

namespace Application.Users.Commands.UpdateUser;

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

        if ( !string.IsNullOrEmpty( command.OldPasswordHash ) && !string.IsNullOrEmpty( command.NewPasswordHash ) )
        {
            string hashedPassword = passwordHasher.GeneratePassword( command.NewPasswordHash );
            user.PasswordHash = hashedPassword;
        }

        await unitOfWork.CommitAsync();

        return Result.FromSuccess();
    }
}