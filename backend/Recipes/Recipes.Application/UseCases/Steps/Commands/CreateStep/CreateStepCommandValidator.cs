using Recipes.Application.Results;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Steps.Commands.CreateStep;

public class CreateStepCommandValidator
    : IAsyncValidator<CreateStepCommand>
{
    public async Task<Result> ValidateAsync( CreateStepCommand command )
    {
        if ( string.IsNullOrEmpty( command.StepDescription ) )
        {
            return Result.FromError( "Описание шага не может быть пустым" );
        }

        if ( command.StepNumber <= 0 )
        {
            return Result.FromError( "Номер шага не может быть меньше или равен нулю" );
        }

        return Result.Success;
    }
}