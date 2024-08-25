using Recipes.Application.CQRSInterfaces;
using Recipes.Domain.Entities;
using Recipes.Application.Results;
using Recipes.Application.Repositories;
using Recipes.Application.Interfaces;
using Recipes.Application.UseCases.Ingredients.Commands.UpdateIngredients;
using Recipes.Application.UseCases.Tags.Commands.UpdateRecipeTags;
using Recipes.Application.UseCases.Steps.Commands.UpdateSteps;
using Recipes.Application.UseCases.Recipes.Commands.CreateRecipe;

namespace Recipes.Application.UseCases.Recipes.Commands.UpdateRecipe;

public class UpdateRecipeCommandHandler(
    IRecipeRepository recipeRepository,
    IAsyncValidator<UpdateRecipeCommand> validator,
    IUnitOfWork unitOfWork,
    ICommandHandler<UpdateStepsCommand> updateStepsCommandHandler,
    ICommandHandler<UpdateIngredientsCommand> updateIngredientsCommandHandler,
    ICommandHandler<UpdateTagsCommand> updateTagsCommandHandler,
    IImageTools imageTools )
    : CommandBaseHandler<UpdateRecipeCommand>( validator )
{
    protected override async Task<Result> HandleAsyncImpl( UpdateRecipeCommand updateRecipeCommand )
    {
        Recipe oldRecipe = await recipeRepository.GetByIdAsync( updateRecipeCommand.Id );
        if ( oldRecipe is null )
        {
            return Result.FromError( "Recipe not found" );
        }

        string oldImageUrl = oldRecipe.ImageUrl;

        oldRecipe.Name = updateRecipeCommand.Name;
        oldRecipe.Description = updateRecipeCommand.Description;
        oldRecipe.PortionCount = updateRecipeCommand.PortionCount;
        oldRecipe.CookTime = updateRecipeCommand.CookTime;
        oldRecipe.ImageUrl = updateRecipeCommand.ImageUrl;

        UpdateStepsCommand updateStepsCommand = new()
        {
            Recipe = oldRecipe,
            NewSteps = updateRecipeCommand.Steps
        };
        await updateStepsCommandHandler.HandleAsync( updateStepsCommand );

        UpdateIngredientsCommand updateIngredientsCommand = new()
        {
            Recipe = oldRecipe,
            NewIngredients = updateRecipeCommand.Ingredients
        };
        await updateIngredientsCommandHandler.HandleAsync( updateIngredientsCommand );

        UpdateTagsCommand updateTagsCommand = new()
        {
            RecipeId = updateRecipeCommand.Id,
            RecipeTags = updateRecipeCommand.Tags
        };
        await updateTagsCommandHandler.HandleAsync( updateTagsCommand );

        await unitOfWork.CommitAsync();

        imageTools.DeleteImage( oldImageUrl );

        return Result.Success;
    }

    protected override async Task HandleExceptionAsync( UpdateRecipeCommand command )
    {
        _ = imageTools.DeleteImage( command.ImageUrl );

        await base.HandleExceptionAsync( command );
    }
}