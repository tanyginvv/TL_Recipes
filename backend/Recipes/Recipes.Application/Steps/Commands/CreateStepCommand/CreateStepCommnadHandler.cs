using Application;
using Application.Repositories;
using Application.Result;
using Application.Validation;
using Recipes.Application.CQRSInterfaces;
using Recipes.Domain.Entities;
using Recipes.Infrastructure.Entities.Steps;

namespace Recipes.Application.Steps.Commands.CreateStepCommand
{
    public class CreateStepCommandHandler : ICommandHandler<CreateStepCommand>
    {
        private readonly IStepRepository _stepRepository;
        private readonly IRecipeRepository _recipeRepository;
        private readonly IAsyncValidator<CreateStepCommand> _validator;
        private readonly IUnitOfWork _unitOfWork;

        public CreateStepCommandHandler(
            IStepRepository stepRepository,
            IRecipeRepository recipeRepository,
            IAsyncValidator<CreateStepCommand> validator,
            IUnitOfWork unitOfWork )
        {
            _stepRepository = stepRepository;
            _recipeRepository = recipeRepository;
            _validator = validator;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> HandleAsync( CreateStepCommand command )
        {
            var validationResult = await _validator.ValidationAsync( command );
            if ( validationResult.IsFail )
            {
                return new CommandResult( validationResult );
            }

            var recipe = await _recipeRepository.GetByIdAsync( command.RecipeId );
            if ( recipe == null )
            {
                return new CommandResult( ValidationResult.Fail( "Recipe not found" ) );
            }

            var step = new Step( command.StepNumber, command.StepDescription )
            {
                RecipeId = command.RecipeId
            };

            await _stepRepository.AddAsync( step );
            await _unitOfWork.CommitAsync();

            return new CommandResult( ValidationResult.Ok() );
        }
    }
}