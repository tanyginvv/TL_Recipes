using Application;
using Application.Repositories;
using Application.Result;
using Application.Validation;
using Recipes.Application.CQRSInterfaces;
using Recipes.Domain.Entities;
using Recipes.Infrastructure.Entities.Steps;
using System.Threading.Tasks;

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

            var recipe = await _recipeRepository.GetByIdAsync( command.RecipeId );
            if ( recipe == null )
            {
                return new CommandResult( ValidationResult.Fail( "Recipe not found" ) );
            }

            var step = await _stepRepository.GetByStepNumberAsync( command.RecipeId, command.StepNumber );
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
