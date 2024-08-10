using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Recipes.Commands.DeleteRecipe;

public class DeleteRecipeCommandValidator(
    IRecipeRepository recipeRepository )
    : IAsyncValidator<DeleteRecipeCommand>
{
    public async Task<Result> ValidateAsync( DeleteRecipeCommand command )
    {
        if ( command.RecipeId <= 0 )
        {
            return Result.FromError( "ID рецепта должно быть больше нуля" );
        }

        Recipe recipe = await recipeRepository.GetByIdAsync( command.RecipeId );

        if ( recipe is null )
        {
            return Result.FromError( "Такого рецепта не существует" );
        }

        if ( recipe.UserId != command.UserId )
        {
            return Result.FromError( "У пользователя нет доступа к удалению данного рецепта" );
        }

        return Result.Success;
    }
}