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
        : ICommandHandler<CreateIngredientCommand>
    {
        public async Task<Result> HandleAsync( CreateIngredientCommand createIngredientCommand )
        {
            Result validationResult = await validator.ValidateAsync( createIngredientCommand );
            if ( !validationResult.IsSuccess )
            {
                return validationResult;
            }

            Ingredient ingredient = new Ingredient(
                createIngredientCommand.Title,
                createIngredientCommand.Description,
                createIngredientCommand.RecipeId );

            await ingredientRepository.AddAsync( ingredient );

            return Result.Success;
        }
    }
}