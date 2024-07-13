using Application.Repositories;
using Application.Validation;

namespace Recipes.Application.Recipes.Commands.UpdateRecipeTags
{
    public class UpdateRecipeTagsCommandValidator : IAsyncValidator<UpdateRecipeTagsCommand>
    {
        private readonly IRecipeRepository _recipeRepository;

        public UpdateRecipeTagsCommandValidator( IRecipeRepository recipeRepository )
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<ValidationResult> ValidationAsync( UpdateRecipeTagsCommand command )
        {
            if ( command.RecipeTags == null || !command.RecipeTags.Any() )
            {
                return ValidationResult.Fail( "Список тегов не может быть пустым" );
            }

            foreach ( var tag in command.RecipeTags )
            {
                if ( tag.Id <= 0 )
                {
                    return ValidationResult.Fail( "ID тега должен быть положительным числом" );
                }
            }

            var recipe = await _recipeRepository.GetByIdAsync( command.RecipeId );
            if ( recipe == null )
            {
                return ValidationResult.Fail( "Рецепт с указанным ID не найден" );
            }

            return ValidationResult.Ok();
        }
    }
}
