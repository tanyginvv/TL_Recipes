using Application;
using Application.Repositories;
using Application.Result;
using Application.Validation;
using Recipes.Application.CQRSInterfaces;
using Recipes.Infrastructure.Repositories;

namespace Recipes.Application.Ingredients.Commands.DeleteIngredient
{
    public class DeleteIngredientCommandHandler : ICommandHandler<DeleteIngredientCommand>
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAsyncValidator<DeleteIngredientCommand> _validator;

        public DeleteIngredientCommandHandler(
            IIngredientRepository ingredientRepository,
            IUnitOfWork unitOfWork,
            IAsyncValidator<DeleteIngredientCommand> validator )
        {
            _ingredientRepository = ingredientRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<CommandResult> HandleAsync( DeleteIngredientCommand command )
        {
            ValidationResult validationResult = await _validator.ValidationAsync( command );
            if ( validationResult.IsFail )
            {
                return new CommandResult( validationResult );
            }

            var ingredient = await _ingredientRepository.GetByRecipeIdAsync( command.Id );
            if ( ingredient == null )
            {
                return new CommandResult( ValidationResult.Fail( "Ingredient not found" ) );
            }

            await _ingredientRepository.DeleteByIdAsync( command.Id );
            await _unitOfWork.CommitAsync();

            return new CommandResult( ValidationResult.Ok() );
        }
    }
}