using Recipes.Application.CQRSInterfaces;
using Recipes.Domain.Entities;
using Recipes.Application.Ingredients.Commands.UpdateIngredient;
using Recipes.Application.Steps.Commands.UpdateStepCommand;
using Recipes.Application.Steps.Commands.DeleteStepCommand;
using Recipes.Application.Ingredients.Commands.CreateIngredient;
using Recipes.Application.Ingredients.Commands.DeleteIngredient;
using Recipes.Application.Recipes.Commands.UpdateRecipeTags;
using Recipes.Application.Validation;
using Recipes.Application.Steps.Commands.CreateStepCommand;
using Recipes.Application.Results;
using Recipes.Application.Repositories;

namespace Recipes.Application.Recipes.Commands.UpdateRecipe
{
    public class UpdateRecipeCommandHandler : ICommandHandler<UpdateRecipeCommand>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IAsyncValidator<UpdateRecipeCommand> _updateRecipeCommandValidator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICommandHandler<UpdateStepCommand> _updateStepCommandHandler;
        private readonly ICommandHandler<UpdateIngredientCommand> _updateIngredientCommandHandler;
        private readonly ICommandHandler<DeleteStepCommand> _deleteStepCommandHandler;
        private readonly ICommandHandler<DeleteIngredientCommand> _deleteIngredientCommandHandler;
        private readonly ICommandHandler<CreateStepCommand> _createStepCommandHandler;
        private readonly ICommandHandler<CreateIngredientCommand> _createIngredientCommandHandler;
        private readonly ICommandHandler<UpdateRecipeTagsCommand> _updateRecipeTagsCommandHandler;

        public UpdateRecipeCommandHandler(
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
        {
            _recipeRepository = recipeRepository;
            _updateRecipeCommandValidator = validator;
            _unitOfWork = unitOfWork;
            _updateStepCommandHandler = updateStepCommandHandler;
            _updateIngredientCommandHandler = updateIngredientCommandHandler;
            _deleteStepCommandHandler = deleteStepCommandHandler;
            _deleteIngredientCommandHandler = deleteIngredientCommandHandler;
            _createStepCommandHandler = createStepCommandHandler;
            _createIngredientCommandHandler = createIngredientCommandHandler;
            _updateStepCommandHandler = updateStepCommandHandler;
            _updateRecipeTagsCommandHandler = updateRecipeTagsCommandHandler;
        }

        public async Task<CommandResult> HandleAsync( UpdateRecipeCommand updateRecipeCommand )
        {
            ValidationResult validationResult = await _updateRecipeCommandValidator.ValidationAsync( updateRecipeCommand );
            if ( validationResult.IsFail )
            {
                return new CommandResult( validationResult );
            }

            Recipe oldRecipe = await _recipeRepository.GetByIdAsync( updateRecipeCommand.Id );
            if ( oldRecipe == null )
            {
                return new CommandResult( ValidationResult.Fail( "Recipe not found" ) );
            }

            oldRecipe.SetName( updateRecipeCommand.Name );
            oldRecipe.SetDescription( updateRecipeCommand.Description );
            oldRecipe.SetCountPortion( updateRecipeCommand.CountPortion );
            oldRecipe.SetCookTime( updateRecipeCommand.CookTime );
            oldRecipe.SetImageUrl( updateRecipeCommand.ImageUrl );

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

            return new CommandResult( ValidationResult.Ok() );

        }
    }
}
