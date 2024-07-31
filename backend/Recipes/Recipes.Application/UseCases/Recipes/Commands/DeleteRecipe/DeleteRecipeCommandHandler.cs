using Recipes.Application.CQRSInterfaces;
using Recipes.Domain.Entities;
using Recipes.Application.Validation;
using Recipes.Application.Results;
using Recipes.Application.Repositories;
using Recipes.Application.Interfaces;

namespace Recipes.Application.UseCases.Recipes.Commands.DeleteRecipe
{
    public class DeleteRecipeCommandHandler(
            IRecipeRepository recipeRepository,
            IAsyncValidator<DeleteRecipeCommand> validator,
            IUnitOfWork unitOfWork,
            IImageTools imageTools )
        : ICommandHandler<DeleteRecipeCommand>
    {
        public async Task<Result> HandleAsync( DeleteRecipeCommand deleteRecipeCommand )
        {
            Result validationResult = await validator.ValidateAsync( deleteRecipeCommand );
            if ( !validationResult.IsSuccess )
            {
                return Result.FromError( validationResult.Error.Message );
            }

            Recipe foundRecipe = await recipeRepository.GetByIdAsync( deleteRecipeCommand.RecipeId );
            if ( foundRecipe is null )
            {
                return Result.FromError( "Рецепт не найден" );
            }

            imageTools.DeleteImage( foundRecipe.ImageUrl );

            await recipeRepository.Delete( foundRecipe );

            await unitOfWork.CommitAsync();

            return Result.Success;
        }
    }
}
