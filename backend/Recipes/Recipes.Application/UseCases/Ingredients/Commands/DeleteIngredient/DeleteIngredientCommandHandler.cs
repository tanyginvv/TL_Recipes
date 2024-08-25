using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Ingredients.Commands.DeleteIngredient;

public class DeleteIngredientCommandHandler(
    IIngredientRepository ingredientRepository,
    IAsyncValidator<DeleteIngredientCommand> validator )
    : CommandBaseHandler<DeleteIngredientCommand>( validator )
{
    protected override async Task<Result> HandleAsyncImpl( DeleteIngredientCommand command )
    {
        Ingredient ingredient = await ingredientRepository.GetByIdAsync( command.Id );
        if ( ingredient is null )
        {
            return Result.FromError( "Ингредиент не найден" );
        }

        await ingredientRepository.Delete( ingredient );

        return Result.Success;
    }
}