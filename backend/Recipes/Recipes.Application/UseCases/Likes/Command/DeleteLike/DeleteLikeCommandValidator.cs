using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.CQRSInterfaces;

namespace Recipes.Application.UseCases.Likes.Command.DeleteLike;

public class DeleteLikeCommandValidator(
    IRecipeRepository recipeRepository,
    IUserRepository userRepository,
    ILikeRepository likeRepository )
    : IAsyncValidator<DeleteLikeCommand>
{
    public async Task<Result> ValidateAsync( DeleteLikeCommand command )
    {
        if ( await recipeRepository.GetByIdAsync( command.RecipeId ) is null )
        {
            return Result.FromError( "Рецепта с таким id не существует" );
        }

        if ( await userRepository.GetByIdAsync( command.UserId ) is null )
        {
            return Result.FromError( "Пользователя с таким id не существует" );
        }

        if ( await likeRepository.GetLikeByAttributes( command.UserId, command.RecipeId ) is null )
        {
            return Result.FromError( "Такого лайка не существует" );
        }

        return Result.Success;
    }
}