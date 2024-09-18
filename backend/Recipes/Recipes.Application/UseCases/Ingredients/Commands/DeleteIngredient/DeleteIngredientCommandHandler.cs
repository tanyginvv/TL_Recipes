using Microsoft.Extensions.Logging;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Ingredients.Commands.DeleteIngredient;

public class DeleteIngredientCommandHandler(
    IIngredientRepository ingredientRepository,
    IAsyncValidator<DeleteIngredientCommand> validator,
    ILogger<DeleteIngredientCommand> logger )
    : CommandBaseHandler<DeleteIngredientCommand>( validator, logger )
{
    protected override async Task<Result> HandleImplAsync( DeleteIngredientCommand command )
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