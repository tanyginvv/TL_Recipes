using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Tags.Commands.UpdateRecipeTags
{
    public class UpdateRecipeTagsCommandValidator( IRecipeRepository recipeRepository )
        : IAsyncValidator<UpdateRecipeTagsCommand>
    {
        public async Task<Result> ValidateAsync( UpdateRecipeTagsCommand command )
        {
            if ( command.RecipeTags is null || !command.RecipeTags.Any() )
            {
                return Result.FromError( "Список тегов не может быть пустым" );
            }

            var recipe = await recipeRepository.GetByIdAsync( command.RecipeId );
            if ( recipe is null )
            {
                return Result.FromError( "Рецепт с указанным ID не найден" );
            }

            return Result.Success;
        }
    }
}
