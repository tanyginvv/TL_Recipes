using Microsoft.Extensions.Logging;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Ingredients.Commands.CreateIngredient;

public class CreateIngredientCommandHandler(
    IIngredientRepository ingredientRepository,
    IAsyncValidator<CreateIngredientCommand> validator,
    ILogger<CreateIngredientCommand> logger )
    : CommandBaseHandlerWithResult<CreateIngredientCommand, Ingredient>( validator, logger )
{
    protected override async Task<Result<Ingredient>> HandleImplAsync( CreateIngredientCommand createIngredientCommand )
    {
        Ingredient ingredient = new Ingredient(
            createIngredientCommand.Title,
            createIngredientCommand.Description,
            createIngredientCommand.Recipe.Id );

        await ingredientRepository.AddAsync( ingredient );

        return Result<Ingredient>.FromSuccess( ingredient );
    }
}