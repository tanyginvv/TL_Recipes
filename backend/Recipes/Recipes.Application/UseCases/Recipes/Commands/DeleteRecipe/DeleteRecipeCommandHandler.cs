using Recipes.Application.CQRSInterfaces;
using Recipes.Domain.Entities;
using Recipes.Application.Results;
using Recipes.Application.Repositories;
using Recipes.Application.Interfaces;

namespace Recipes.Application.UseCases.Recipes.Commands.DeleteRecipe;

public class DeleteRecipeCommandHandler(
    IRecipeRepository recipeRepository,
    IAsyncValidator<DeleteRecipeCommand> validator,
    IUnitOfWork unitOfWork,
    IImageTools imageTools )
    : CommandBaseHandler<DeleteRecipeCommand>( validator )
{
    protected override async Task<Result> HandleImplAsync( DeleteRecipeCommand deleteRecipeCommand )
    {
        Recipe foundRecipe = await recipeRepository.GetByIdAsync( deleteRecipeCommand.RecipeId );
        if ( foundRecipe is null )
        {
            return Result.FromError( "Рецепт не найден" );
        }

        await recipeRepository.Delete( foundRecipe );
        await unitOfWork.CommitAsync();

        imageTools.DeleteImage( foundRecipe.ImageUrl );

        return Result.Success;
    }
}