using Application.Validation;
using Recipes.Infrastructure.Repositories;

namespace Recipes.Application.Steps
{
    public class CreateStepCommandValidator : IAsyncValidator<CreateStepCommand>
    {
        private readonly IRecipeRepository _recipeRepository;

        public CreateStepCommandValidator( IRecipeRepository recipeRepository )
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<ValidationResult> ValidationAsync( CreateStepCommand command )
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