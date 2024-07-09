using Application.Repositories;
using Application.Validation;
using Recipes.Application.Steps.Dtos;

namespace Recipes.Application.Steps.Commands
{
    public class CreateStepCommandValidator : IAsyncValidator<CreateStepCommandDto>
    {
        private readonly IRecipeRepository _recipeRepository;

        public CreateStepCommandValidator( IRecipeRepository recipeRepository )
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<ValidationResult> ValidationAsync( CreateStepCommandDto command )
        {
            if ( string.IsNullOrWhiteSpace( command.StepDescription ) )
            {
                return ValidationResult.Fail( "Step description cannot be empty" );
            }

            if ( command.RecipeId <= 0 )
            {
                return ValidationResult.Fail( "Recipe ID must be a non-negative integer" );
            }

            var recipeExists = await _recipeRepository.GetByIdAsync( command.RecipeId ) != null;
            if ( !recipeExists )
            {
                return ValidationResult.Fail( "Recipe not found" );
            }

            return ValidationResult.Ok();
        }
    }
}