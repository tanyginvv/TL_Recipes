using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.CQRSInterfaces;

namespace Recipes.Application.UseCases.Likes.Command.CreateLike;

public class CreateLikeCommandValidator(
    IRecipeRepository recipeRepository,
    IUserRepository userRepository,
    ILikeRepository likeRepository )
    : IAsyncValidator<CreateLikeCommand>
{
    public async Task<Result> ValidateAsync( CreateLikeCommand command )
    {
        if ( await recipeRepository.GetByIdAsync( command.RecipeId ) is null )
        {
            return Result.FromError( "Рецепта с таким id не существует" );
        }

        if ( await userRepository.GetByIdAsync( command.UserId ) is null )
        {
            return Result.FromError( "Пользователя с таким id не существует" );
        }

        if ( await likeRepository.GetLikeByAttributes( command.UserId, command.RecipeId ) is not null )
        {
            return Result.FromError( "Такой лайк уже существует" );
        }

        return Result.Success;
    }
}