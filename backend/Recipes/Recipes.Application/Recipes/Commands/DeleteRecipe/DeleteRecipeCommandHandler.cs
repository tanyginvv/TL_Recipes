using Application.Result;
using Application.Validation;
using Application;
using Recipes.Application.CQRSInterfaces;
using Application.Repositories;
using Recipes.Domain.Entities;

namespace Recipes.Application.Recipes.Commands.DeleteRecipe
{
    public class DeleteRecipeCommandHandler : ICommandHandler<DeleteRecipeCommand>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IAsyncValidator<DeleteRecipeCommand> _deleteRecipeCommandValidator;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteRecipeCommandHandler(
            IRecipeRepository recipeRepository,
            IAsyncValidator<DeleteRecipeCommand> validator,
            IUnitOfWork unitOfWork )
        {
            _recipeRepository = recipeRepository;
            _deleteRecipeCommandValidator = validator;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> HandleAsync( DeleteRecipeCommand deleteRecipeCommand )
        {
            ValidationResult validationResult = await _deleteRecipeCommandValidator.ValidationAsync( deleteRecipeCommand );
            if ( !validationResult.IsFail )
            {
                Recipe foundRecipe = await _recipeRepository.GetByIdAsync( deleteRecipeCommand.RecipeId );
                _ = _recipeRepository.DeleteAsync( foundRecipe.Id );
                await _unitOfWork.CommitAsync();
            }
            return new CommandResult( validationResult );
        }
    }
}