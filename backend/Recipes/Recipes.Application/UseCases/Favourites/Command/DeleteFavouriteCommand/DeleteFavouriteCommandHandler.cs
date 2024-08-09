using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Interfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Likes.Command;

public class DeleteFavouriteCommandHandler(
    IFavouriteRepository repository,
    IUnitOfWork unitOfWork,
    IAsyncValidator<DeleteFavouriteCommand> validator )
    : ICommandHandler<DeleteFavouriteCommand>
{
    public async Task<Result> HandleAsync( DeleteFavouriteCommand command )
    {
        Result result = await validator.ValidateAsync( command );
        if ( !result.IsSuccess )
        {
            return Result.FromError( result.Error );
        }

        Favourite favourite = await repository.GetFavouriteByAttributes( command.RecipeId, command.UserId );

        if ( favourite is null )
        {
            return Result.FromError( "Избранное не найдено" );
        }
        await repository.Delete( favourite );
        await unitOfWork.CommitAsync();

        return Result.Success;
    }
}
