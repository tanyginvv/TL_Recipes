using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Steps.Commands.UpdateStep;

public class UpdateStepCommandHandler(
    IStepRepository stepRepository,
    IAsyncValidator<UpdateStepCommand> validator )
    : CommandBaseHandler<UpdateStepCommand>( validator )
{
    protected override async Task<Result> HandleAsyncImpl( UpdateStepCommand command )
    {
        Step step = await stepRepository.GetByStepIdAsync( command.StepId );
        if ( step is null || step.Id != command.StepId )
        {
            return Result.FromError( "Шаг не найден или не относится к указанному рецепту." );
        }

        step.StepNumber = command.StepNumber;
        step.StepDescription = command.StepDescription;

        return Result.FromSuccess();
    }
}