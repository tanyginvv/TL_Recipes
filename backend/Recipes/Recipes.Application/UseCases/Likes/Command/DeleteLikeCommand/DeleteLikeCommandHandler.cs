using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Interfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Likes.Command;

public class DeleteLikeCommandHandler(
    ILikeRepository repository,
    IUnitOfWork unitOfWork,
    IAsyncValidator<DeleteLikeCommand> validator )
    : ICommandHandler<DeleteLikeCommand>
{
    public async Task<Result> HandleAsync( DeleteLikeCommand command )
    {
        Result result = await validator.ValidateAsync( command );
        if ( !result.IsSuccess )
        {
            return Result.FromError( result.Error );
        }

        Like like = await repository.GetLikeByAttributes( command.UserId, command.RecipeId );

        if ( like is null )
        {
            return Result.FromError( "Лайк не найден" );
        }

        await repository.Delete( like );
        await unitOfWork.CommitAsync();

        return Result.Success;
    }
}
