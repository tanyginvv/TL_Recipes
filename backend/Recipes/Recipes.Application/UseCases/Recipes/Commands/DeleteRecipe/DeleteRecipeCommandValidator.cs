using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
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
            return Result.FromError( "Рецепт не найден" );
        }

        if ( recipe.AuthorId != command.AuthorId )
        {
            return Result.FromError( "У пользователя нет доступа к удалению данного рецепта" );
        }

        return Result.Success;
    }
}