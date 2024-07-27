using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Steps.Commands.CreateStepCommand
{
    public class CreateStepCommandHandler(
            IStepRepository stepRepository,
            IRecipeRepository recipeRepository,
            IAsyncValidator<CreateStepCommand> validator )
        : ICommandHandler<CreateStepCommand>
    {
        public async Task<Result> HandleAsync( CreateStepCommand command )
        {
            Result validationResult = await validator.ValidateAsync( command );
            if ( !validationResult.IsSuccess )
            {
                return Result.FromError( validationResult.Error );
            }

            Recipe recipe = await recipeRepository.GetByIdAsync( command.RecipeId );
            if ( recipe is null )
            {
                return Result.FromError( "Recipe not found" );
            }

            Step step = new( command.StepNumber, command.StepDescription, command.RecipeId );

            await stepRepository.AddAsync( step );

            return Result.Success;
        }
    }
}