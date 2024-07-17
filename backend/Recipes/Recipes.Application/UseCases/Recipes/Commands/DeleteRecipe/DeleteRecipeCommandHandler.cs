using Recipes.Application.CQRSInterfaces;
using Recipes.Domain.Entities;
using Recipes.Application.Validation;
using Recipes.Application.Results;
using Recipes.Application.Repositories;
using Recipes.Application.UseCases.Steps.Commands.DeleteStepCommand;
using Recipes.Application.UseCases.Ingredients.Commands.DeleteIngredient;

namespace Recipes.Application.UseCases.Recipes.Commands.DeleteRecipe
{
    public class DeleteRecipeCommandHandler(
            IRecipeRepository recipeRepository,
            IAsyncValidator<DeleteRecipeCommand> validator,
            IUnitOfWork unitOfWork,
            ICommandHandler<DeleteIngredientCommand> deleteIngredientCommandHandler,
            ICommandHandler<DeleteStepCommand> deleteStepCommandHandler )
        : ICommandHandler<DeleteRecipeCommand>
    {
        private IRecipeRepository _recipeRepository => recipeRepository;
        private IAsyncValidator<DeleteRecipeCommand> _deleteRecipeCommandValidator => validator;
        private IUnitOfWork _unitOfWork => unitOfWork;
        private ICommandHandler<DeleteIngredientCommand> _deleteIngredientCommandHandler => deleteIngredientCommandHandler;
        private ICommandHandler<DeleteStepCommand> _deleteStepCommandHandler => deleteStepCommandHandler;

        public async Task<Result> HandleAsync( DeleteRecipeCommand deleteRecipeCommand )
        {
            Result validationResult = await _deleteRecipeCommandValidator.ValidationAsync( deleteRecipeCommand );
            if ( !validationResult.IsSuccess )
            {
                return Result.FromError( validationResult.Error.Message );
            }

            Recipe foundRecipe = await _recipeRepository.GetByIdAsync( deleteRecipeCommand.RecipeId );
            if ( foundRecipe == null )
            {
                return Result.FromError( "Recipe not found" );
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

            return Result.Success;
        }
    }
}
