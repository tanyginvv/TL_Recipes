using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Likes.Queries.GetLikeBoolRecipeAndUser;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Likes.Queries.GetLikesCountForRecipeQuery
{
    public class GetLikeBoolRecipeAndUserQueryValidator(
        IRecipeRepository recipeRepository,
        IUserRepository userRepository
        ) : IAsyncValidator<GetLikeBoolRecipeAndUserQuery>
    {
        public async Task<Result> ValidateAsync( GetLikeBoolRecipeAndUserQuery command )
        {
            if ( await recipeRepository.GetByIdAsync( command.RecipeId ) is null )
            {
                return Result.FromError( "Рецепта с таким id не существует" );
            }

            if ( await userRepository.GetByIdAsync( command.UserId ) is null )
            {
                return Result.FromError( "Пользователя с таким id не существует" );
            }

            return Result.Success;
        }
    }
}
