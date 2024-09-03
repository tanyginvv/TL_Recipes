using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.CQRSInterfaces;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Favourites.Command.CreateFavourite;

public class CreateFovouriteCommandValidator(
    IRecipeRepository recipeRepository,
    IUserRepository userRepository,
    IFavouriteRepository favouriteRepository )
    : IAsyncValidator<CreateFavouriteCommand>
{
    public async Task<Result> ValidateAsync( CreateFavouriteCommand command )
    {
        Recipe recipe = await recipeRepository.GetByIdAsync( command.RecipeId );
        if ( recipe is null )
        {
            return Result.FromError( "Рецепта с таким id не существует" );
        }

        if ( await userRepository.GetByIdAsync( command.UserId ) is null )
        {
            return Result.FromError( "Пользователя с таким id не существует" );
        }

        if ( await favouriteRepository.ContainsAsync( u => u.UserId == command.UserId && u.RecipeId == command.RecipeId ) )
        {
            return Result.FromError( "Такое избранное уже существует" );
        }

        return Result.Success;
    }
}