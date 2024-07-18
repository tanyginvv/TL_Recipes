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
            IAsyncValidator<CreateStepCommand> validator,
            IUnitOfWork unitOfWork ) : ICommandHandler<CreateStepCommand>
    {
        private IStepRepository _stepRepository => stepRepository;
        private IRecipeRepository _recipeRepository => recipeRepository;
        private IAsyncValidator<CreateStepCommand> _validator => validator;
        private IUnitOfWork _unitOfWork => unitOfWork;

        public async Task<Result> HandleAsync( CreateStepCommand command )
        {
            var validationResult = await _validator.ValidationAsync( command );
            if ( !validationResult.IsSuccess )
            {
                return Result.FromError( validationResult.Error );
            }

            var recipe = await _recipeRepository.GetByIdAsync( command.RecipeId );
            if ( recipe == null )
            {
                return Result.FromError( "Recipe not found" );
            }

            var step = new Step( command.StepNumber, command.StepDescription, command.RecipeId );

            await _stepRepository.AddAsync( step );
            await _unitOfWork.CommitAsync();

            return Result.Success;
        }
    }
}