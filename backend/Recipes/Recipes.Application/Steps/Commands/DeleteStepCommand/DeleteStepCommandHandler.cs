using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;

namespace Recipes.Application.Steps.Commands.DeleteStepCommand
{
    public class DeleteStepCommandHandler : ICommandHandler<DeleteStepCommand>
    {
        private readonly IStepRepository _stepRepository;
        private readonly IRecipeRepository _recipeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAsyncValidator<DeleteStepCommand> _validator;

        public DeleteStepCommandHandler(
            IStepRepository stepRepository,
            IRecipeRepository recipeRepository,
            IUnitOfWork unitOfWork,
            IAsyncValidator<DeleteStepCommand> validator )
        {
            _stepRepository = stepRepository;
            _recipeRepository = recipeRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<CommandResult> HandleAsync( DeleteStepCommand command )
        {
            ValidationResult validationResult = await _validator.ValidationAsync( command );
            if ( validationResult.IsFail )
            {
                return new CommandResult( validationResult );
            }

            var step = await _stepRepository.GetByStepIdAsync( command.StepId );
            if ( step == null )
            {
                return new CommandResult( ValidationResult.Fail( "Step not found" ) );
            }

            if ( step.Id != command.StepId )
            {
                return new CommandResult( ValidationResult.Fail( "Step ID does not match the specified step number" ) );
            }

            await _stepRepository.DeleteAsync( step.Id );
            await _unitOfWork.CommitAsync();

            return new CommandResult( ValidationResult.Ok() );
        }
    }
}
