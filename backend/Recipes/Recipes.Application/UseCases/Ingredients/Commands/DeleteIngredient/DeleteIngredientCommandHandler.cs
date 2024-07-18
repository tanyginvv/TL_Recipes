using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Ingredients.Commands.DeleteIngredient
{
    public class DeleteIngredientCommandHandler(
            IIngredientRepository ingredientRepository,
            IUnitOfWork unitOfWork,
            IAsyncValidator<DeleteIngredientCommand> validator )
        : ICommandHandler<DeleteIngredientCommand>
    {
        private IIngredientRepository _ingredientRepository => ingredientRepository;
        private IUnitOfWork _unitOfWork => unitOfWork;
        private IAsyncValidator<DeleteIngredientCommand> _validator => validator;

        public async Task<Result> HandleAsync( DeleteIngredientCommand command )
        {
            Result validationResult = await _validator.ValidationAsync( command );
            if ( !validationResult.IsSuccess )
            {
                return Result.FromError( validationResult.Error );
            }

            var ingredient = await _ingredientRepository.GetByIdAsync( command.Id );
            if ( ingredient == null )
            {
                return Result.FromError( "Ingredient not found" );
            }

            await _ingredientRepository.DeleteByIdAsync( command.Id );
            await _unitOfWork.CommitAsync();

            return Result.Success;
        }
    }
}
