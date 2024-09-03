using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.CQRSInterfaces;

namespace Recipes.Application.UseCases.Favourites.Command.DeleteFavourite;

public class DeleteFavouriteCommandValidator(
    IRecipeRepository recipeRepository,
    IUserRepository userRepository,
    IFavouriteRepository favouriteRepository )
    : IAsyncValidator<DeleteFavouriteCommand>
{
    public async Task<Result> ValidateAsync( DeleteFavouriteCommand command )
    {
        if ( await recipeRepository.GetByIdAsync( command.RecipeId ) is null )
        {
            return Result.FromError( "Рецепта с таким id не существует" );
        }

        if ( await userRepository.GetByIdAsync( command.UserId ) is null )
        {
            return Result.FromError( "Пользователя с таким id не существует" );
        }

        if ( !await favouriteRepository.ContainsAsync( u => u.UserId == command.UserId && u.RecipeId == command.RecipeId ) )
        {
            return Result.FromError( "Такого избранного не существует" );
        }

        return Result.Success;
    }
}
