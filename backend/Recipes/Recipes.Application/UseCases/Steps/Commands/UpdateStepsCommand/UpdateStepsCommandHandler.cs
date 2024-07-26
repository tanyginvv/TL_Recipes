using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Steps.Commands
{
    public class UpdateStepsCommandHandler(
        ICommandHandler<UpdateStepCommand> updateStepCommandHandler,
        ICommandHandler<DeleteStepCommand> deleteStepCommandHandler,
        ICommandHandlerWithResult<CreateStepCommand, Step> createStepCommandHandler,
        IAsyncValidator<UpdateStepsCommand> validator )
        : ICommandHandler<UpdateStepsCommand>
    {
        public async Task<Result> HandleAsync( UpdateStepsCommand command )
        {
            Result validationResult = await validator.ValidateAsync( command );
            if ( !validationResult.IsSuccess )
            {
                return Result.FromError( validationResult.Error );
            }

            List<Step> oldSteps = command.Recipe.Steps.ToList();

            foreach ( StepDto newStep in command.NewSteps )
            {
                Step existingStep = oldSteps.FirstOrDefault( oldStep => oldStep.StepNumber == newStep.StepNumber );
                if ( existingStep is null )
                {
                    CreateStepCommand createStepCommand = new CreateStepCommand
                    {
                        Recipe = command.Recipe,
                        StepDescription = newStep.StepDescription,
                        StepNumber = newStep.StepNumber
                    };
                    await createStepCommandHandler.HandleAsync( createStepCommand );
                }
                else if ( existingStep.StepDescription != newStep.StepDescription )
                {
                    UpdateStepCommand updateStepCommand = new UpdateStepCommand
                    {
                        StepId = existingStep.Id,
                        StepDescription = newStep.StepDescription,
                        StepNumber = newStep.StepNumber
                    };
                    await updateStepCommandHandler.HandleAsync( updateStepCommand );
                }
            }

            List<Step> stepsToDelete = oldSteps.Where( oldStep => !command.NewSteps.Any( newStep => newStep.StepNumber == oldStep.StepNumber ) ).ToList();
            foreach ( Step stepToDelete in stepsToDelete )
            {
                var deleteStepCommand = new DeleteStepCommand { StepId = stepToDelete.Id };
                await deleteStepCommandHandler.HandleAsync( deleteStepCommand );
            }

            return Result.Success;
        }
    }
}
