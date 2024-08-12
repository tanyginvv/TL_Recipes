using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Steps.Commands;

public class UpdateStepCommandValidator(
    IStepRepository stepRepository )
    : IAsyncValidator<UpdateStepCommand>
{
    public async Task<Result> ValidateAsync( UpdateStepCommand command )
    {
        if ( command.StepId <= 0 )
        {
            return Result.FromError( "ID шага должен быть больше нуля" );
        }

        if ( command.StepNumber <= 0 )
        {
            return Result.FromError( "Номер шага должен быть больше нуля" );
        }

        if ( string.IsNullOrEmpty( command.StepDescription ) )
        {
            return Result.FromError( "Описание шага не может быть пустым" );
        }

        Step step = await stepRepository.GetByStepIdAsync( command.StepId );
        if ( step is null || step.Id != command.StepId )
        {
            return Result.FromError( "Шаг не найден или не относится к указанному рецепту." );
        }

        return Result.Success;
    }
}