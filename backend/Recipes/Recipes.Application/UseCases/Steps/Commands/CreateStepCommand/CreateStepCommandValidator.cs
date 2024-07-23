using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Steps.Commands.CreateStepCommand
{
    public class CreateStepCommandValidator( IRecipeRepository recipeRepository )
        : IAsyncValidator<CreateStepCommand>
    {
        public async Task<Result> ValidateAsync( CreateStepCommand command )
        {
            if ( string.IsNullOrEmpty( command.StepDescription ) )
            {
                return Result.FromError( "Описание шага не может быть пустым" );
            }

            if ( command.RecipeId <= 0 )
            {
                return Result.FromError( "ID рецепта должен быть больше 0" );
            }

            var recipeExists = await recipeRepository.GetByIdAsync( command.RecipeId );
            if ( recipeExists is null )
            {
                return Result.FromError( "Рецепт не найден" );
            }

            return Result.Success;
        }
    }
}