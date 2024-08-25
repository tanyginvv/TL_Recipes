using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Steps.Commands.DeleteStep;

public class DeleteStepCommandHandler(
    IStepRepository stepRepository,
    IAsyncValidator<DeleteStepCommand> validator )
    : CommandBaseHandler<DeleteStepCommand>( validator )
{
    protected override async Task<Result> HandleAsyncImpl( DeleteStepCommand command )
    {
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

        return Result.FromSuccess();
    }
}