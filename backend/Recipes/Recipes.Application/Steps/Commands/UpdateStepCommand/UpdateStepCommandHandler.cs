using Application.Result;
using Application.Validation;
using Recipes.Application.CQRSInterfaces;
using Recipes.Infrastructure.Repositories;

namespace Recipes.Application.Steps.Commands.UpdateStepCommand
{
    public class UpdateStepCommandHandler : ICommandHandler<UpdateStepCommand>
    {
        private readonly IStepRepository _stepRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAsyncValidator<UpdateStepCommand> _validator;

        public UpdateStepCommandHandler(
            IStepRepository stepRepository,
            IUnitOfWork unitOfWork,
            IAsyncValidator<UpdateStepCommand> validator )
        {
            _stepRepository = stepRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<CommandResult> HandleAsync( UpdateStepCommand command )
        {
            ValidationResult validationResult = await _validator.ValidationAsync( command );
            if ( validationResult.IsFail )
            {
                return new CommandResult( validationResult );
            }

            var step = await _stepRepository.GetByStepIdAsync( command.StepId );
            if ( step == null || step.Id != command.StepId )
            {
                return new CommandResult( ValidationResult.Fail( "Step not found or does not belong to the specified recipe." ) );
            }

            step.SetStepNumber( command.StepNumber );
            step.SetStepDescription( command.StepDescription );

            await _stepRepository.UpdateAsync( step );
            await _unitOfWork.CommitAsync();

            return new CommandResult( ValidationResult.Ok() );
        }
    }
}
