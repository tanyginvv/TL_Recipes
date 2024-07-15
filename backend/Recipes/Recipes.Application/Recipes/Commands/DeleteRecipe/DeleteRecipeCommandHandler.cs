using Recipes.Application.CQRSInterfaces;
using Recipes.Domain.Entities;
using Recipes.Application.Ingredients.Commands.DeleteIngredient;
using Recipes.Application.Steps.Commands.DeleteStepCommand;
using Recipes.Application.Validation;
using Recipes.Application.Results;
using Recipes.Application.Repositories;

namespace Recipes.Application.Recipes.Commands.DeleteRecipe
{
    public class DeleteRecipeCommandHandler : ICommandHandler<DeleteRecipeCommand>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IAsyncValidator<DeleteRecipeCommand> _deleteRecipeCommandValidator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICommandHandler<DeleteIngredientCommand> _deleteIngredientCommandHandler;
        private readonly ICommandHandler<DeleteStepCommand> _deleteStepCommandHandler;

        public DeleteRecipeCommandHandler(
            IRecipeRepository recipeRepository,
            IAsyncValidator<DeleteRecipeCommand> validator,
            IUnitOfWork unitOfWork,
            ICommandHandler<DeleteIngredientCommand> deleteIngredientCommandHandler,
            ICommandHandler<DeleteStepCommand> deleteStepCommandHandler )
        {
            _recipeRepository = recipeRepository;
            _deleteRecipeCommandValidator = validator;
            _unitOfWork = unitOfWork;
            _deleteIngredientCommandHandler = deleteIngredientCommandHandler;
            _deleteStepCommandHandler = deleteStepCommandHandler;
        }

        public async Task<CommandResult> HandleAsync( DeleteRecipeCommand deleteRecipeCommand )
        {
            ValidationResult validationResult = await _deleteRecipeCommandValidator.ValidationAsync( deleteRecipeCommand );
            if ( validationResult.IsFail )
            {
                return new CommandResult( validationResult );
            }

            Recipe foundRecipe = await _recipeRepository.GetByIdAsync( deleteRecipeCommand.RecipeId );
            if ( foundRecipe == null )
            {
                return new CommandResult( ValidationResult.Fail( "Recipe not found" ) );
            }

            foreach ( var ingredientDto in foundRecipe.Ingredients.ToList() )
            {
                var deleteIngredientCommand = new DeleteIngredientCommand
                {
                    Id = ingredientDto.Id
                };
                await _deleteIngredientCommandHandler.HandleAsync( deleteIngredientCommand );
            }

            foreach ( var stepDto in foundRecipe.Steps.ToList() )
            {
                var deleteStepCommand = new DeleteStepCommand
                {
                    StepId = stepDto.Id
                };
                await _deleteStepCommandHandler.HandleAsync( deleteStepCommand );
            }

            await _recipeRepository.DeleteAsync( foundRecipe.Id );

            await _unitOfWork.CommitAsync();

            return new CommandResult( ValidationResult.Ok() );
        }
    }
}
