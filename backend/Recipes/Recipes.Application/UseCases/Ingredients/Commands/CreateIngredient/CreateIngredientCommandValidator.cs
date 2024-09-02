using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Results;

namespace Recipes.Application.UseCases.Ingredients.Commands.CreateIngredient;

public class CreateIngredientCommandValidator : IAsyncValidator<CreateIngredientCommand>
{
    public async Task<Result> ValidateAsync( CreateIngredientCommand command )
    {
        if ( string.IsNullOrEmpty( command.Title ) )
        {
            return Result.FromError( "Название ингредиента не может быть пустым" );
        }

        if ( command.Title.Length > 100 )
        {
            return Result.FromError( "Название ингредиента не может быть больше чем 100 символов" );
        }

        if ( string.IsNullOrEmpty( command.Description ) )
        {
            return Result.FromError( "Описание ингредиента не может быть пустым" );
        }

        if ( command.Description.Length > 250 )
        {
            return Result.FromError( "Описание ингредиента не может быть больше чем 250 символов" );
        }

        return Result.Success;
    }
}