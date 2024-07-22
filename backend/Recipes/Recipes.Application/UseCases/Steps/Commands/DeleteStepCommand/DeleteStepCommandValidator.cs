using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Steps.Commands.DeleteStepCommand
{
    public class DeleteStepCommandValidator( IStepRepository stepRepository )
        : IAsyncValidator<DeleteStepCommand>
    {
        public async Task<Result> ValidateAsync( DeleteStepCommand command )
        {
            Step step = await stepRepository.GetByStepIdAsync( command.StepId );
            if ( step is null )
            {
                return Result.FromError( "Step not found" );
            }

            if ( step.Id != command.StepId )
            {
                return Result.FromError( "Step ID does not match the specified step number" );
            }

            return Result.Success;
        }
    }
}