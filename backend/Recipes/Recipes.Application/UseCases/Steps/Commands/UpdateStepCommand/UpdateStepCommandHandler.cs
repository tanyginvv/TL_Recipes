using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Steps.Commands.UpdateStepCommand
{
    public class UpdateStepCommandHandler(
            IStepRepository stepRepository,
            IUnitOfWork unitOfWork,
            IAsyncValidator<UpdateStepCommand> validator )
        : ICommandHandler<UpdateStepCommand>
    {
        private IStepRepository _stepRepository => stepRepository;
        private IUnitOfWork _unitOfWork => unitOfWork;
        private IAsyncValidator<UpdateStepCommand> _validator => validator;

        public async Task<Result> HandleAsync( UpdateStepCommand command )
        {
            Result validationResult = await _validator.ValidationAsync( command );
            if ( !validationResult.IsSuccess )
            {
                return Result.FromError( validationResult.Error );
            }

            var step = await _stepRepository.GetByStepIdAsync( command.StepId );
            if ( step == null || step.Id != command.StepId )
            {
                return Result.FromError( "Step not found or does not belong to the specified recipe." );
            }

            step.StepNumber = command.StepNumber;
            step.StepDescription = command.StepDescription;

            await _stepRepository.UpdateAsync( step );
            await _unitOfWork.CommitAsync();

            return Result.Success;
        }
    }
}
