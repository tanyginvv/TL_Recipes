using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Ingredients.Commands.UpdateIngredient
{
    public class UpdateIngredientCommandHandler(
           IIngredientRepository ingredientRepository,
           IAsyncValidator<UpdateIngredientCommand> validator,
           IUnitOfWork unitOfWork )
        : ICommandHandler<UpdateIngredientCommand>
    {
        private IIngredientRepository _ingredientRepository => ingredientRepository;
        private IAsyncValidator<UpdateIngredientCommand> _updateIngredientCommandValidator => validator;
        private IUnitOfWork _unitOfWork => unitOfWork;

        public async Task<Result> HandleAsync( UpdateIngredientCommand updateIngredientCommand )
        {
            Result validationResult = await _updateIngredientCommandValidator.ValidationAsync( updateIngredientCommand );
            if ( !validationResult.IsSuccess )
            {
                Ingredient ingredient = await _ingredientRepository.GetByIdAsync( updateIngredientCommand.Id );
                if ( ingredient != null )
                {
                    ingredient.Title = updateIngredientCommand.Title;
                    ingredient.Description = updateIngredientCommand.Description;
                    await _ingredientRepository.UpdateIngredientAsync( ingredient );
                    await _unitOfWork.CommitAsync();
                }
                else
                {
                    return Result.FromError( "Такого id не существует" );
                }
            }
            return Result.FromSuccess();
        }
    }
}
