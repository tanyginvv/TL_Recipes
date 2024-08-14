using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Interfaces;
using Recipes.Application.PasswordHasher;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Users.Commands;

public class CreateUserCommandHandler(
    IUserRepository userRepository,
    IAsyncValidator<CreateUserCommand> validator,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher ) 
    : ICommandHandler<CreateUserCommand>
{
    public async Task<Result> HandleAsync( CreateUserCommand command )
    {
        Result result = await validator.ValidateAsync( command );
        if ( !result.IsSuccess )
        {
            return Result.FromError( result.Error );
        }

        string hashedPassword = passwordHasher.GeneratePassword( command.PasswordHash );

        User user = new User( command.Name, command.Login, hashedPassword, command.Description );
        await userRepository.AddAsync( user );
        await unitOfWork.CommitAsync();

        return Result.FromSuccess();
    }
}