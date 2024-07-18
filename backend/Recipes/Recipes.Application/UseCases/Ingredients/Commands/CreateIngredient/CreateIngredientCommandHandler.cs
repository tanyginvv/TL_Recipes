using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Ingredients.Commands.CreateIngredient
{
    public class CreateIngredientCommandHandler(
            IIngredientRepository ingredientRepository,
            IAsyncValidator<CreateIngredientCommand> validator,
            IUnitOfWork unitOfWork )
        : ICommandHandler<CreateIngredientCommand>
    {
        private IIngredientRepository _ingredientRepository => ingredientRepository;
        private IAsyncValidator<CreateIngredientCommand> _createIngredientCommandValidator => validator;
        private IUnitOfWork _unitOfWork => unitOfWork;

        public async Task<Result> HandleAsync( CreateIngredientCommand createIngredientCommand )
        {
            Result validationResult = await _createIngredientCommandValidator.ValidationAsync( createIngredientCommand );
            if ( !validationResult.IsSuccess )
            {
                return validationResult;
            }

            Ingredient ingredient = new Ingredient(
                createIngredientCommand.Title,
                createIngredientCommand.Description,
                createIngredientCommand.RecipeId );

            await _ingredientRepository.AddIngredientAsync( ingredient );
            await _unitOfWork.CommitAsync();

            return Result.Success;
        }
    }
}
