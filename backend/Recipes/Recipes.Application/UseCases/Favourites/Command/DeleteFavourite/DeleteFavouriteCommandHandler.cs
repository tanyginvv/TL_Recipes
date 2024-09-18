using Microsoft.Extensions.Logging;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Interfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Favourites.Command.DeleteFavourite;

public class DeleteFavouriteCommandHandler(
    IFavouriteRepository repository,
    IUnitOfWork unitOfWork,
    IAsyncValidator<DeleteFavouriteCommand> validator,
    ILogger<DeleteFavouriteCommand> logger )
    : CommandBaseHandler<DeleteFavouriteCommand>( validator, logger )
{
    protected override async Task<Result> HandleImplAsync( DeleteFavouriteCommand command )
    {
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