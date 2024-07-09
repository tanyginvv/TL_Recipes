using Application.Result;
using Application.Validation;
using Application;
using Recipes.Application.CQRSInterfaces;
using Application.Repositories;
using Recipes.Domain.Entities;

namespace Recipes.Application.Recipes.Commands.UpdateRecipe
{
    public class UpdateRecipeCommandHandler : ICommandHandler<UpdateRecipeCommand>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IAsyncValidator<UpdateRecipeCommand> _updateRecipeCommandValidator;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateRecipeCommandHandler(
            IRecipeRepository recipeRepository,
            IAsyncValidator<UpdateRecipeCommand> validator,
            IUnitOfWork unitOfWork )
        {
            _recipeRepository = recipeRepository;
            _updateRecipeCommandValidator = validator;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> HandleAsync( UpdateRecipeCommand updateRecipeCommand )
        {
            ValidationResult validationResult = await _updateRecipeCommandValidator.ValidationAsync( updateRecipeCommand );
            if ( !validationResult.IsFail )
            {
                Recipe oldRecipe = await _recipeRepository.GetByIdAsync( updateRecipeCommand.Id );
                oldRecipe.SetName( updateRecipeCommand.Name );
                oldRecipe.SetDescription( updateRecipeCommand.Description );
                oldRecipe.SetCountPortion( updateRecipeCommand.CountPortion );
                oldRecipe.SetCookTime( updateRecipeCommand.CookTime );
                oldRecipe.SetImageUrl( updateRecipeCommand.ImageUrl );
                await _unitOfWork.CommitAsync();
            }
            return new CommandResult( validationResult );
        }
    }
}
