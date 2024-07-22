using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Steps.Commands.CreateStepCommand
{
    public class CreateStepCommandValidator( IRecipeRepository recipeRepository )
        : IAsyncValidator<CreateStepCommand>
    {
        public async Task<Result> ValidateAsync( CreateStepCommand command )
        {
            if ( string.IsNullOrWhiteSpace( command.StepDescription ) )
            {
                return Result.FromError( "Step description cannot be empty" );
            }

            if ( command.RecipeId <= 0 )
            {
                return Result.FromError( "Recipe ID must be a non-negative integer" );
            }

            var recipeExists = await recipeRepository.GetByIdAsync( command.RecipeId );
            if ( recipeExists is null )
            {
                return Result.FromError( "Recipe not found" );
            }

            return Result.Success;
        }
    }
}