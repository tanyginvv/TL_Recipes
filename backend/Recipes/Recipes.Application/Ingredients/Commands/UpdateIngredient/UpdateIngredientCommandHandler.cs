using Application;
using Application.Result;
using Application.Validation;
using Recipes.Application.CQRSInterfaces;
using Recipes.Domain.Entities;
using Recipes.Infrastructure.Repositories;

namespace Recipes.Application.Ingredients.Commands.UpdateIngredient
{
    public class UpdateIngredientCommandHandler : ICommandHandler<UpdateIngredientCommand>
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IAsyncValidator<UpdateIngredientCommand> _updateIngredientCommandValidator;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateIngredientCommandHandler(
           IIngredientRepository ingredientRepository,
           IAsyncValidator<UpdateIngredientCommand> validator,
           IUnitOfWork unitOfWork )
        {
            _ingredientRepository = ingredientRepository;
            _updateIngredientCommandValidator = validator;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> HandleAsync( UpdateIngredientCommand updateIngredientCommand )
        {
            ValidationResult validationResult = await _updateIngredientCommandValidator.ValidationAsync( updateIngredientCommand );
            if ( !validationResult.IsFail )
            {
                Ingredient ingredient = await _ingredientRepository.GetByIdAsync( updateIngredientCommand.Id );
                if ( ingredient != null )
                {
                    ingredient.SetTitle( updateIngredientCommand.Title );
                    ingredient.SetDescription( updateIngredientCommand.Description );
                    await _ingredientRepository.UpdateIngredientAsync( ingredient );
                    await _unitOfWork.CommitAsync();
                }
                else
                {
                    return new CommandResult( ValidationResult.Fail( "Такого id не существует" ) );
                }
            }
            return new CommandResult( validationResult );
        }
    }
}
