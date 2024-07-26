using Recipes.Application.CQRSInterfaces;
using Recipes.Domain.Entities;
using Recipes.Application.Validation;
using Recipes.Application.Results;
using Recipes.Application.Repositories;
using Recipes.Application.Interfaces;
using Recipes.Application.UseCases.Ingredients.Commands;
using Recipes.Application.UseCases.Tags.Commands;
using Recipes.Application.UseCases.Steps.Commands;

namespace Recipes.Application.UseCases.Recipes.Commands.UpdateRecipe
{
    public class UpdateRecipeCommandHandler(
            IRecipeRepository recipeRepository,
    IAsyncValidator<UpdateRecipeCommand> validator,
            IUnitOfWork unitOfWork,
            ICommandHandler<UpdateStepsCommand> updateStepsCommandHandler,
            ICommandHandler<UpdateIngredientsCommand> updateIngredientsCommandHandler,
            ICommandHandler<UpdateTagsCommand> updateTagsCommandHandler )
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
                return Result.FromError( "Рецепт не найден" );
            }

            oldRecipe.Name = updateRecipeCommand.Name;
            oldRecipe.Description = updateRecipeCommand.Description;
            oldRecipe.PortionCount = updateRecipeCommand.PortionCount;
            oldRecipe.CookTime = updateRecipeCommand.CookTime;
            oldRecipe.ImageUrl = updateRecipeCommand.ImageUrl;

            UpdateStepsCommand updateStepsCommand = new UpdateStepsCommand
            {
                Recipe = oldRecipe,
                NewSteps = updateRecipeCommand.Steps
            };
            await updateStepsCommandHandler.HandleAsync( updateStepsCommand );

            var updateIngredientsCommand = new UpdateIngredientsCommand
            {
                Recipe = oldRecipe,
                NewIngredients = updateRecipeCommand.Ingredients
            };
            await updateIngredientsCommandHandler.HandleAsync( updateIngredientsCommand );

            UpdateTagsCommand updateTagsCommand = new UpdateTagsCommand
            {
                RecipeId = updateRecipeCommand.Id,
                RecipeTags = updateRecipeCommand.Tags
            };
            await updateTagsCommandHandler.HandleAsync( updateTagsCommand );

            await unitOfWork.CommitAsync();

            return Result.Success;
        }

    }
}
