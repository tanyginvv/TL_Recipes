using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Tags.Commands.UpdateRecipeTags
{
    public class UpdateTagsCommandValidator( IRecipeRepository recipeRepository )
        : IAsyncValidator<UpdateTagsCommand>
    {
        public async Task<Result> ValidateAsync( UpdateTagsCommand command )
        {
            if ( command.RecipeTags is null || !command.RecipeTags.Any() )
            {
                return Result.FromError( "Список тегов не может быть пустым" );
            }

            Recipe recipe = await recipeRepository.GetByIdAsync( command.RecipeId );
            if ( recipe is null )
            {
                return Result.FromError( "Рецепт с указанным ID не найден" );
            }

            foreach ( TagDto tag in command.RecipeTags )
            {
                if ( tag.Name.Length > 50 )
                {
                    return Result.FromError( "Название тега не может быть больше 50 символов" );
                }
            }

            return Result.Success;
        }
    }
}
