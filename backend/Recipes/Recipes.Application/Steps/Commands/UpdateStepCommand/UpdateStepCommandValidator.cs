using Application.Validation;
using Recipes.Application.Steps.Commands.UpdateStepCommand;
using Recipes.Infrastructure.Entities.Steps;
using System.Threading.Tasks;

namespace Recipes.Application.Steps.Commands.UpdateStepCommand
{
    public class UpdateStepCommandValidator : IAsyncValidator<UpdateStepCommand>
    {
        private readonly IStepRepository _stepRepository;

        public UpdateStepCommandValidator( IStepRepository stepRepository )
        {
            _stepRepository = stepRepository;
        }

        public async Task<ValidationResult> ValidationAsync( UpdateStepCommand command )
        {
            if ( command.RecipeId <= 0 )
            {
                return ValidationResult.Fail( "RecipeId must be greater than zero." );
            }

            if ( command.StepId <= 0 )
            {
                return ValidationResult.Fail( "StepId must be greater than zero." );
            }

            if ( command.StepNumber <= 0 )
            {
                return ValidationResult.Fail( "StepNumber must be greater than zero." );
            }

            if ( string.IsNullOrWhiteSpace( command.StepDescription ) )
            {
                return ValidationResult.Fail( "StepDescription cannot be empty." );
            }

            var step = await _stepRepository.GetByStepNumberAsync( command.RecipeId, command.StepNumber );
            if ( step == null || step.Id != command.StepId )
            {
                return ValidationResult.Fail( "Step not found or does not belong to the specified recipe." );
            }

            return ValidationResult.Ok();
        }
    }
}