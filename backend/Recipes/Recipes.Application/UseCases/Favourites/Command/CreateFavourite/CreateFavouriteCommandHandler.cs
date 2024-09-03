using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Interfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Favourites.Command.CreateFavourite;

public class CreateFavouriteCommandHandler(
    IFavouriteRepository repository,
    IUnitOfWork unitOfWork,
    IAsyncValidator<CreateFavouriteCommand> validator )
    : CommandBaseHandler<CreateFavouriteCommand>( validator )
{
    protected override async Task<Result> HandleImplAsync( CreateFavouriteCommand command )
    {
        Favourite favourite = new( command.RecipeId, command.UserId );

        await repository.AddAsync( favourite );
        await unitOfWork.CommitAsync();

        return Result.Success;
    }
}