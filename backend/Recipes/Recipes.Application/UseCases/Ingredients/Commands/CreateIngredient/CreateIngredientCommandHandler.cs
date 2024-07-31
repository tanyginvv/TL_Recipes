using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Ingredients.Commands.CreateIngredient
{
    public class CreateIngredientCommandHandler(
            IIngredientRepository ingredientRepository,
            IAsyncValidator<CreateIngredientCommand> validator )
        : ICommandHandlerWithResult<CreateIngredientCommand, Ingredient>
    {
        public async Task<Result<Ingredient>> HandleAsync( CreateIngredientCommand createIngredientCommand )
        {
            Result validationResult = await validator.ValidateAsync( createIngredientCommand );
            if ( !validationResult.IsSuccess )
            {
                return Result<Ingredient>.FromError( validationResult.Error );
            }

            Ingredient ingredient = new Ingredient(
                createIngredientCommand.Title,
                createIngredientCommand.Description,
                createIngredientCommand.Recipe.Id );

            await ingredientRepository.AddAsync( ingredient );

            return Result<Ingredient>.FromSuccess( ingredient );
        }
    }
}