using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Interfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Likes.Command;

public class CreateLikeCommandHandler(
    ILikeRepository repository,
    IUnitOfWork unitOfWork,
    IAsyncValidator<CreateLikeCommand> validator )
    : ICommandHandler<CreateLikeCommand>
{
    public async Task<Result> HandleAsync( CreateLikeCommand command )
    {
        Result result = await validator.ValidateAsync( command );
        if ( !result.IsSuccess )
        {
            return Result.FromError( result.Error );
        }

        Like like = new Like( command.RecipeId, command.UserId );

        await repository.AddAsync( like );
        await unitOfWork.CommitAsync();

        return Result.Success;
    }
}
