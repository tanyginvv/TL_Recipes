using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Steps.Commands.DeleteStepCommand
{
    public class DeleteStepCommandHandler(
            IStepRepository stepRepository,
            IUnitOfWork unitOfWork,
            IAsyncValidator<DeleteStepCommand> validator ) : ICommandHandler<DeleteStepCommand>
    {
        private IStepRepository _stepRepository => stepRepository;
        private IUnitOfWork _unitOfWork => unitOfWork;
        private IAsyncValidator<DeleteStepCommand> _validator => validator;

        public async Task<Result> HandleAsync( DeleteStepCommand command )
        {
            Result validationResult = await _validator.ValidationAsync( command );
            if ( !validationResult.IsSuccess )
            {
                return Result.FromError( validationResult.Error );
            }

            var step = await _stepRepository.GetByStepIdAsync( command.StepId );
            if ( step == null )
            {
                return Result.FromError( "Step not found" );
            }

            if ( step.Id != command.StepId )
            {
                return Result.FromError( "Step ID does not match the specified step number" );
            }

            await _stepRepository.DeleteAsync( step.Id );
            await _unitOfWork.CommitAsync();

            return Result.Success;
        }
    }
}
