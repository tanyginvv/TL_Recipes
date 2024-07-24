using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Steps.Commands
{
    public class CreateStepCommandHandler(
            IStepRepository stepRepository,
            IRecipeRepository recipeRepository,
            IAsyncValidator<CreateStepCommand> validator )
        : ICommandHandlerWithResult<CreateStepCommand, Step>
    {
        public async Task<Result<Step>> HandleAsync( CreateStepCommand command )
        {
            Result validationResult = await validator.ValidateAsync( command );
            if ( !validationResult.IsSuccess )
            {
                return Result<Step>.FromError( validationResult.Error );
            }

            Step step = new( command.StepNumber, command.StepDescription, command.RecipeId );

            await stepRepository.AddAsync( step );

            return Result<Step>.FromSuccess( step );
        }
    }
}