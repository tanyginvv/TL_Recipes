using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Interfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler(
    IUserRepository userRepository,
    IAsyncValidator<DeleteUserCommand> validator,
    IUnitOfWork unitOfWork )
   : CommandBaseHandler<DeleteUserCommand>( validator )
{
    protected override async Task<Result> HandleAsyncImpl( DeleteUserCommand command )
    {
        User user = await userRepository.GetByIdAsync( command.UserId );

        await userRepository.Delete( user );
        await unitOfWork.CommitAsync();

        return Result.FromSuccess();
    }
}