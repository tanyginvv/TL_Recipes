using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Interfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Likes.Command
{
    public class CreateFavouriteCommandHandler(
        IFavouriteRepository repository,
        IUnitOfWork unitOfWork,
        IAsyncValidator<CreateFavouriteCommand> validator )
        : ICommandHandler<CreateFavouriteCommand>
    {
        public async Task<Result> HandleAsync( CreateFavouriteCommand command )
        {
            Result result = await validator.ValidateAsync( command );
            if ( !result.IsSuccess )
            {
                return Result.FromError( result.Error );
            }

            Favourite favourite = new( command.RecipeId, command.UserId );

            await repository.AddAsync( favourite );
            await unitOfWork.CommitAsync();

            return Result.Success;
        }
    }
}
