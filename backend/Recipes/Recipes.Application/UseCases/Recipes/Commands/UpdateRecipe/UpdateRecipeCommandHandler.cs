using Recipes.Application.CQRSInterfaces;
using Recipes.Domain.Entities;
using Recipes.Application.Validation;
using Recipes.Application.Results;
using Recipes.Application.Repositories;
using Recipes.Application.UseCases.Steps.Commands.CreateStepCommand;
using Recipes.Application.UseCases.Steps.Commands.DeleteStepCommand;
using Recipes.Application.UseCases.Steps.Commands.UpdateStepCommand;
using Recipes.Application.UseCases.Ingredients.Commands.CreateIngredient;
using Recipes.Application.UseCases.Ingredients.Commands.DeleteIngredient;
using Recipes.Application.UseCases.Ingredients.Commands.UpdateIngredient;
using Recipes.Application.UseCases.Recipes.Commands.UpdateRecipeTags;
using Recipes.Application.UseCases.Recipes.Commands.CreateRecipe;

namespace Recipes.Application.UseCases.Recipes.Commands.UpdateRecipe
{
    public class UpdateRecipeCommandHandler(
            IRecipeRepository recipeRepository,
            IAsyncValidator<UpdateRecipeCommand> validator,
            IUnitOfWork unitOfWork,
            ICommandHandler<UpdateStepCommand> updateStepCommandHandler,
            ICommandHandler<UpdateIngredientCommand> updateIngredientCommandHandler,
            ICommandHandler<DeleteStepCommand> deleteStepCommandHandler,
            ICommandHandler<DeleteIngredientCommand> deleteIngredientCommandHandler,
            ICommandHandler<CreateStepCommand> createStepCommandHandler,
            ICommandHandler<CreateIngredientCommand> createIngredientCommandHandler,
            ICommandHandler<UpdateRecipeTagsCommand> updateRecipeTagsCommandHandler )
        : ICommandHandler<UpdateRecipeCommand>
    {
        private IRecipeRepository _recipeRepository => recipeRepository;
        private IAsyncValidator<UpdateRecipeCommand> _updateRecipeCommandValidator => validator;
        private IUnitOfWork _unitOfWork => unitOfWork;
        private ICommandHandler<UpdateStepCommand> _updateStepCommandHandler => updateStepCommandHandler;
        private ICommandHandler<UpdateIngredientCommand> _updateIngredientCommandHandler => updateIngredientCommandHandler;
        private ICommandHandler<DeleteStepCommand> _deleteStepCommandHandler => deleteStepCommandHandler;
        private ICommandHandler<DeleteIngredientCommand> _deleteIngredientCommandHandler => deleteIngredientCommandHandler;
        private ICommandHandler<CreateStepCommand> _createStepCommandHandler => createStepCommandHandler;
        private ICommandHandler<CreateIngredientCommand> _createIngredientCommandHandler => createIngredientCommandHandler;
        private ICommandHandler<UpdateRecipeTagsCommand> _updateRecipeTagsCommandHandler => updateRecipeTagsCommandHandler;

        public async Task<Result> HandleAsync( UpdateRecipeCommand updateRecipeCommand )
        {
            Result validationResult = await _updateRecipeCommandValidator.ValidationAsync( updateRecipeCommand );
            if ( !validationResult.IsSuccess )
            {
                return Result.FromError( validationResult.Error );
            }

            Recipe oldRecipe = await _recipeRepository.GetByIdAsync( updateRecipeCommand.Id );
            if ( oldRecipe == null )
            {
                return Result.FromError( "Recipe not found" );
            }

            oldRecipe.Name = updateRecipeCommand.Name;
            oldRecipe.Description = updateRecipeCommand.Description;
            oldRecipe.CountPortion = updateRecipeCommand.CountPortion;
            oldRecipe.CookTime = updateRecipeCommand.CookTime;
            oldRecipe.ImageUrl = updateRecipeCommand.ImageUrl;

            var oldSteps = oldRecipe.Steps.ToList();
            var newSteps = updateRecipeCommand.Steps.ToList();

            for ( int i = 0; i < Math.Min( oldSteps.Count, newSteps.Count ); i++ )
            {
                var updateStepCommand = new UpdateStepCommand()
                {
                    StepId = oldSteps[ i ].Id,
                    StepDescription = newSteps[ i ].StepDescription,
                    StepNumber = newSteps[ i ].StepNumber
                };
                await _updateStepCommandHandler.HandleAsync( updateStepCommand );
            }

            for ( int i = newSteps.Count; i < oldSteps.Count; i++ )
            {
                var deleteStepCommand = new DeleteStepCommand() { StepId = oldSteps[ i ].Id };
                await _deleteStepCommandHandler.HandleAsync( deleteStepCommand );
            }

            for ( int i = oldSteps.Count; i < newSteps.Count; i++ )
            {
                var createStepCommand = new CreateStepCommand()
                {
                    RecipeId = updateRecipeCommand.Id,
                    StepDescription = newSteps[ i ].StepDescription,
                    StepNumber = newSteps[ i ].StepNumber
                };
                await _createStepCommandHandler.HandleAsync( createStepCommand );
            }

            var oldIngredients = oldRecipe.Ingredients.ToList();
            var newIngredients = updateRecipeCommand.Ingredients.ToList();

            for ( int i = 0; i < Math.Min( oldIngredients.Count, newIngredients.Count ); i++ )
            {
                var updateIngredientCommand = new UpdateIngredientCommand()
                {
                    Id = oldIngredients[ i ].Id,
                    Title = newIngredients[ i ].Title,
                    Description = newIngredients[ i ].Description
                };
                await _updateIngredientCommandHandler.HandleAsync( updateIngredientCommand );
            }

            for ( int i = newIngredients.Count; i < oldIngredients.Count; i++ )
            {
                var deleteIngredientCommand = new DeleteIngredientCommand() { Id = oldIngredients[ i ].Id };
                await _deleteIngredientCommandHandler.HandleAsync( deleteIngredientCommand );
            }

            for ( int i = oldIngredients.Count; i < newIngredients.Count; i++ )
            {
                var createIngredientCommand = new CreateIngredientCommand()
                {
                    RecipeId = updateRecipeCommand.Id,
                    Title = newIngredients[ i ].Title,
                    Description = newIngredients[ i ].Description
                };
                await _createIngredientCommandHandler.HandleAsync( createIngredientCommand );
            }

            var newTagIds = updateRecipeCommand.Tags.ToList();
            var newTagCommand = new UpdateRecipeTagsCommand
            {
                RecipeId = updateRecipeCommand.Id,
                RecipeTags = newTagIds
            };

            await _updateRecipeTagsCommandHandler.HandleAsync( newTagCommand );

            await _unitOfWork.CommitAsync();

            return Result.Success;

        }
    }
}
