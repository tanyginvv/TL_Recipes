using Application;
using Application.Result;
using Application.Validation;
using Recipes.Application.CQRSInterfaces;
using Recipes.Domain.Entities;
using Recipes.Infrastructure.Repositories;

namespace Recipes.Application.Ingredients.Commands.CreateIngredient
{
    public class CreateIngredientCommandHandler : ICommandHandler<CreateIngredientCommand>
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IAsyncValidator<CreateIngredientCommand> _createIngredientCommandValidator;
        private readonly IUnitOfWork _unitOfWork;

        public CreateIngredientCommandHandler(
            IIngredientRepository ingredientRepository,
            IAsyncValidator<CreateIngredientCommand> validator,
            IUnitOfWork unitOfWork )
        {
            _ingredientRepository = ingredientRepository;
            _createIngredientCommandValidator = validator;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> HandleAsync( CreateIngredientCommand createIngredientCommand )
        {
            ValidationResult validationResult = await _createIngredientCommandValidator.ValidationAsync( createIngredientCommand );
            if ( !validationResult.IsFail )
            {
                Ingredient ingredient = new Ingredient(
                    createIngredientCommand.Title,
                    createIngredientCommand.Description );
                await _ingredientRepository.AddIngredientAsync( ingredient );
                await _unitOfWork.CommitAsync();
            }
            return new CommandResult( validationResult );
        }
    }
}
