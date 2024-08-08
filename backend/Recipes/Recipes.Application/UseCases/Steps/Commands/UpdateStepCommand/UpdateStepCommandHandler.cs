using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Steps.Commands
{
    public class UpdateStepCommandHandler(
        IStepRepository stepRepository,
        IAsyncValidator<UpdateStepCommand> validator )
        : ICommandHandler<UpdateStepCommand>
    {
        public async Task<Result> HandleAsync( UpdateStepCommand command )
        {
            Result validationResult = await validator.ValidateAsync( command );
            if ( !validationResult.IsSuccess )
            {
                return Result.FromError( validationResult.Error );
            }

            Step step = await stepRepository.GetByStepIdAsync( command.StepId );
            if ( step is null || step.Id != command.StepId )
            {
                return Result.FromError( "Шаг не найден или не относится к указанному рецепту." );
            }

            step.StepNumber = command.StepNumber;
            step.StepDescription = command.StepDescription;

            return Result.Success;
        }
    }
}