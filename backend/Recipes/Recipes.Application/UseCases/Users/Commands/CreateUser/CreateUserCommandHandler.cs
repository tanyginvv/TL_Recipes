using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Interfaces;
using Recipes.Application.PasswordHasher;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Users.Commands.CreateUser;

public class CreateUserCommandHandler(
    IUserRepository userRepository,
    IAsyncValidator<CreateUserCommand> validator,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher )
    : CommandBaseHandler<CreateUserCommand>( validator )
{
    protected override async Task<Result> HandleImplAsync( CreateUserCommand command )
    {
        string hashedPassword = passwordHasher.GeneratePassword( command.Password );

        User user = new User( command.Name, command.Login, hashedPassword );
        await userRepository.AddAsync( user );
        await unitOfWork.CommitAsync();

        return Result.FromSuccess();
    }
}