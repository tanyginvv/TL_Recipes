using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Steps.Commands.DeleteStepCommand
{
    public class DeleteStepCommandValidator( IStepRepository stepRepository )
        : IAsyncValidator<DeleteStepCommand>
    {
        private IStepRepository _stepRepository => stepRepository;

        public async Task<Result> ValidationAsync( DeleteStepCommand command )
        {
            var step = await _stepRepository.GetByStepIdAsync( command.StepId );
            if ( step == null )
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