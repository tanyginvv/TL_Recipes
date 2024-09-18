using Recipes.Application.Interfaces;
using Recipes.Application.Repositories;
using Recipes.Domain.Entities;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Results;
using Recipes.Application.PasswordHasher;
using Microsoft.Extensions.Logging;

namespace Recipes.Application.UseCases.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler(
    IUserRepository userRepository,
    IAsyncValidator<UpdateUserCommand> validator,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    ILogger<UpdateUserCommand> logger )
    : CommandBaseHandler<UpdateUserCommand>( validator, logger )
{
    protected override async Task<Result> HandleImplAsync( UpdateUserCommand command )
    {
        User user = await userRepository.GetByIdAsync( command.Id );
        if ( user is null )
        {
            return Result.FromError( "Пользователь не найден." );
        }

        if ( !string.IsNullOrWhiteSpace( command.Name ) )
        {
            user.Name = command.Name;
        }

        if ( command.Description is not null )
        {
            user.Description = command.Description;
        }

        if ( !string.IsNullOrWhiteSpace( command.Login ) )
        {
            user.Login = command.Login;
        }

        if ( !string.IsNullOrWhiteSpace( command.OldPassword ) && !string.IsNullOrWhiteSpace( command.NewPassword ) )
        {
            string hashedPassword = passwordHasher.GeneratePassword( command.NewPassword );
            user.PasswordHash = hashedPassword;
        }

        await unitOfWork.CommitAsync();

        return Result.FromSuccess();
    }
}