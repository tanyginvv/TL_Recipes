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
            if ( command.RecipeId <= 0 )
            {
                return ValidationResult.Fail( "Recipe ID must be greater than zero" );
            }

            if ( command.StepNumber <= 0 )
            {
                return ValidationResult.Fail( "Step number must be greater than zero" );
            }

            var recipe = await _recipeRepository.GetByIdAsync( command.RecipeId );
            if ( recipe == null )
            {
                return ValidationResult.Fail( "Recipe not found" );
            }

            var step = await _stepRepository.GetByStepNumberAsync( command.RecipeId, command.StepNumber );
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