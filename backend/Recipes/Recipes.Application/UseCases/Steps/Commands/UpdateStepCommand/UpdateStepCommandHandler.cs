using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Steps.Commands.UpdateStepCommand
{
    public class UpdateStepCommandHandler(
            IStepRepository stepRepository,
            IAsyncValidator<UpdateStepCommand> validator )
        : ICommandHandler<UpdateStepCommand>
    {
        public async Task<Result> HandleAsync( UpdateStepCommand command )
        {
            Result validationResult = await validator.ValidateAsync( command );
            if ( !validationResult.IsSuccess )
            {
                return Result.FromError( validationResult.Error );
            }

            Step step = await stepRepository.GetByStepIdAsync( command.StepId );
            if ( step is null || step.Id != command.StepId )
            {
                return Result.FromError( "Step not found or does not belong to the specified recipe." );
            }

            step.StepNumber = command.StepNumber;
            step.StepDescription = command.StepDescription;

            await stepRepository.UpdateAsync( step );

            return Result.Success;
        }
    }
}
