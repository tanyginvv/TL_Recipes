using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Steps.Commands.UpdateStepCommand
{
    public class UpdateStepCommandValidator( IStepRepository stepRepository )
        : IAsyncValidator<UpdateStepCommand>
    {
        public async Task<Result> ValidateAsync( UpdateStepCommand command )
        {
            if ( command.StepId <= 0 )
            {
                return Result.FromError( "StepId must be greater than zero." );
            }

            if ( command.StepNumber <= 0 )
            {
                return Result.FromError( "StepNumber must be greater than zero." );
            }

            if ( string.IsNullOrWhiteSpace( command.StepDescription ) )
            {
                return Result.FromError( "StepDescription cannot be empty." );
            }

            Step step = await stepRepository.GetByStepIdAsync( command.StepId );
            if ( step is null || step.Id != command.StepId )
            {
                return Result.FromError( "Step not found or does not belong to the specified recipe." );
            }

            return Result.Success;
        }
    }
}