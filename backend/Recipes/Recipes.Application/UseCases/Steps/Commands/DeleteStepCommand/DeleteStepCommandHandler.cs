using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Steps.Commands;

public class DeleteStepCommandHandler(
    IStepRepository stepRepository,
    IAsyncValidator<DeleteStepCommand> validator )
    : ICommandHandler<DeleteStepCommand>
{
    public async Task<Result> HandleAsync( DeleteStepCommand command )
    {
        Result validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsSuccess )
        {
            return Result.FromError( validationResult.Error );
        }

        Step step = await stepRepository.GetByStepIdAsync( command.StepId );
        if ( step is null )
        {
            return Result.FromError( "Шаг не найден" );
        }

        if ( step.Id != command.StepId )
        {
            return Result.FromError( "ID шага не соответствует указанному номеру шага" );
        }

        await stepRepository.Delete( step );

        return Result.Success;
    }
}