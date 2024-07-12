using Application.Repositories;
using Application.Validation;
using Recipes.Infrastructure.Entities.Steps;

namespace Recipes.Application.Steps.Commands.DeleteStepCommand
{
    public class DeleteStepCommandValidator : IAsyncValidator<DeleteStepCommand>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IStepRepository _stepRepository;

        public DeleteStepCommandValidator( IRecipeRepository recipeRepository, IStepRepository stepRepository )
        {
            _recipeRepository = recipeRepository;
            _stepRepository = stepRepository;
        }

        public async Task<ValidationResult> ValidationAsync( DeleteStepCommand command )
        {
            var step = await _stepRepository.GetByStepIdAsync( command.StepId );
            if ( step == null )
            {
                return ValidationResult.Fail( "Step not found" );
            }

            if ( step.Id != command.StepId )
            {
                return ValidationResult.Fail( "Step ID does not match the specified step number" );
            }

            return ValidationResult.Ok();
        }
    }
}