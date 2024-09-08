using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Interfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Likes.Command.DeleteLike;

public class DeleteLikeCommandHandler(
    ILikeRepository repository,
    IUnitOfWork unitOfWork,
    IAsyncValidator<DeleteLikeCommand> validator )
    : CommandBaseHandler<DeleteLikeCommand>( validator )
{
    protected override async Task<Result> HandleImplAsync( DeleteLikeCommand command )
    {
        Like like = await repository.GetLikeByAttributes( command.RecipeId, command.UserId );

        if ( like is null )
        {
            return Result.FromError( "Лайк не найден" );
        }

        await repository.Delete( like );
        await unitOfWork.CommitAsync();

        return Result.Success;
    }
}