using Recipes.Application.CQRSInterfaces;
using Recipes.Domain.Entities;
using Recipes.Application.Validation;
using Recipes.Application.Results;
using Recipes.Application.Repositories;
using Recipes.Application.UseCases.Steps.Commands.CreateStepCommand;
using Recipes.Application.UseCases.Steps.Commands.DeleteStepCommand;
using Recipes.Application.UseCases.Steps.Commands.UpdateStepCommand;
using Recipes.Application.UseCases.Ingredients.Commands.CreateIngredient;
using Recipes.Application.UseCases.Ingredients.Commands.UpdateIngredient;
using Recipes.Application.UseCases.Recipes.Commands.UpdateRecipeTags;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Application.UseCases.Ingredients.Commands.DeleteIngredient;

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
        public async Task<Result> HandleAsync( UpdateRecipeCommand updateRecipeCommand )
        {
            Result validationResult = await validator.ValidateAsync( updateRecipeCommand );
            if ( !validationResult.IsSuccess )
            {
                return Result.FromError( validationResult.Error );
            }

            Recipe oldRecipe = await recipeRepository.GetByIdAsync( updateRecipeCommand.Id );
            if ( oldRecipe is null )
            {
                return Result.FromError( "Recipe not found" );
            }

            oldRecipe.Name = updateRecipeCommand.Name;
            oldRecipe.Description = updateRecipeCommand.Description;
            oldRecipe.PortionCount = updateRecipeCommand.PortionCount;
            oldRecipe.CookTime = updateRecipeCommand.CookTime;
            oldRecipe.ImageUrl = updateRecipeCommand.ImageUrl;

            List<Step> oldSteps = oldRecipe.Steps.ToList();
            List<StepDto> newSteps = updateRecipeCommand.Steps.ToList();

            for ( int i = 0; i < Math.Min( oldSteps.Count, newSteps.Count ); i++ )
            {
                UpdateStepCommand updateStepCommand = new()
                {
                    StepId = oldSteps[ i ].Id,
                    StepDescription = newSteps[ i ].StepDescription,
                    StepNumber = newSteps[ i ].StepNumber
                };
                await updateStepCommandHandler.HandleAsync( updateStepCommand );
            }

            for ( int i = newSteps.Count; i < oldSteps.Count; i++ )
            {
                DeleteStepCommand deleteStepCommand = new() { StepId = oldSteps[ i ].Id };
                await deleteStepCommandHandler.HandleAsync( deleteStepCommand );
            }

            for ( int i = oldSteps.Count; i < newSteps.Count; i++ )
            {
                CreateStepCommand createStepCommand = new()
                {
                    RecipeId = updateRecipeCommand.Id,
                    StepDescription = newSteps[ i ].StepDescription,
                    StepNumber = newSteps[ i ].StepNumber
                };
                await createStepCommandHandler.HandleAsync( createStepCommand );
            }

            List<Ingredient> oldIngredients = oldRecipe.Ingredients.ToList();
            List<IngredientDto> newIngredients = updateRecipeCommand.Ingredients.ToList();

            for ( int i = 0; i < Math.Min( oldIngredients.Count, newIngredients.Count ); i++ )
            {
                UpdateIngredientCommand updateIngredientCommand = new()
                {
                    Id = oldIngredients[ i ].Id,
                    Title = newIngredients[ i ].Title,
                    Description = newIngredients[ i ].Description
                };
                await updateIngredientCommandHandler.HandleAsync( updateIngredientCommand );
            }

            for ( int i = newIngredients.Count; i < oldIngredients.Count; i++ )
            {
                DeleteIngredientCommand deleteIngredientCommand = new() { Id = oldIngredients[ i ].Id };
                await deleteIngredientCommandHandler.HandleAsync( deleteIngredientCommand );
            }

            for ( int i = oldIngredients.Count; i < newIngredients.Count; i++ )
            {
                CreateIngredientCommand createIngredientCommand = new()
                {
                    RecipeId = updateRecipeCommand.Id,
                    Title = newIngredients[ i ].Title,
                    Description = newIngredients[ i ].Description
                };
                await createIngredientCommandHandler.HandleAsync( createIngredientCommand );
            }

            List<TagDto> newTagIds = updateRecipeCommand.Tags.ToList();
            UpdateRecipeTagsCommand newTagCommand = new()
            {
                RecipeId = updateRecipeCommand.Id,
                RecipeTags = newTagIds
            };

            await updateRecipeTagsCommandHandler.HandleAsync( newTagCommand );

            await unitOfWork.CommitAsync();

            return Result.Success;
        }
    }
}
