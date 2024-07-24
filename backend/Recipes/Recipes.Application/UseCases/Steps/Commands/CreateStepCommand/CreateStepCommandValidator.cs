using Recipes.Application.Results;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Steps.Commands
{
    public class CreateStepCommandValidator
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

            return Result.Success;
        }
    }
}