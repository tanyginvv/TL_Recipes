using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Interfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler(
    IUserRepository userRepository,
    IAsyncValidator<DeleteUserCommand> validator,
    IUnitOfWork unitOfWork ) 
    : ICommandHandler<DeleteUserCommand>
{
    public async Task<Result> HandleAsync( DeleteUserCommand command )
    {
        Result result = await validator.ValidateAsync( command );
        if ( !result.IsSuccess )
        {
            return Result.FromError( result.Error );
        }

        User user = await userRepository.GetByIdAsync( command.UserId );
        await userRepository.Delete( user );
        await unitOfWork.CommitAsync();

        return Result.FromSuccess();
    }
}