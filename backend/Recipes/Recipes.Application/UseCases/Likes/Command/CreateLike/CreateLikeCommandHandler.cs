using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Interfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Likes.Command.CreateLike;

public class CreateLikeCommandHandler(
    ILikeRepository repository,
    IUnitOfWork unitOfWork,
    IAsyncValidator<CreateLikeCommand> validator )
    : CommandBaseHandler<CreateLikeCommand>( validator )
{
    protected override async Task<Result> HandleImplAsync( CreateLikeCommand command )
    {
        Like like = new Like( command.RecipeId, command.UserId );

        await repository.AddAsync( like );
        await unitOfWork.CommitAsync();

        return Result.Success;
    }
}