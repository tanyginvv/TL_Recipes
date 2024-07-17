using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Recipes.Commands.UpdateRecipeTags
{
    public class UpdateRecipeTagsCommandValidator( IRecipeRepository recipeRepository ) : IAsyncValidator<UpdateRecipeTagsCommand>
    {
        private IRecipeRepository _recipeRepository => recipeRepository;

        public async Task<Result> ValidationAsync( UpdateRecipeTagsCommand command )
        {
            if ( command.RecipeTags == null || !command.RecipeTags.Any() )
            {
                return Result.FromError( "Список тегов не может быть пустым" );
            }

            var recipe = await _recipeRepository.GetByIdAsync( command.RecipeId );
            if ( recipe is null )
            {
                return Result.FromError( "Рецепт с указанным ID не найден" );
            }

            return Result.Success;
        }
    }
}
