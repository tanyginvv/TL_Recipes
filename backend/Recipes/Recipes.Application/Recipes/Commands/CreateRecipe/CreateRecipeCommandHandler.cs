using Application;
using Application.Repositories;
using Application.Result;
using Application.Validation;
using Recipes.Application.CQRSInterfaces;
using Recipes.Domain.Entities;

namespace Recipes.Application.Recipes.Commands.CreateRecipe
{
    public class CreateRecipeCommandHandler : ICommandHandler<CreateRecipeCommand>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IAsyncValidator<CreateRecipeCommand> _createRecipeCommandValidator;
        private readonly IUnitOfWork _unitOfWork;

        public CreateRecipeCommandHandler(
           IRecipeRepository recipeRepository,
           IAsyncValidator<CreateRecipeCommand> validator,
           IUnitOfWork unitOfWork )
        {
            _recipeRepository = recipeRepository;
            _createRecipeCommandValidator = validator;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> HandleAsync( CreateRecipeCommand createRecipeCommand )
        {
            ValidationResult validationResult = await _createRecipeCommandValidator.ValidationAsync( createRecipeCommand );
            if ( !validationResult.IsFail )
            {
                Recipe recipe = new Recipe(
                    createRecipeCommand.Name,
                    createRecipeCommand.Description,
                    createRecipeCommand.CookTime,
                    createRecipeCommand.CountPortion,
                    createRecipeCommand.ImageUrl );
                await _recipeRepository.AddAsync( recipe );
                await _unitOfWork.CommitAsync();
            }
            return new CommandResult( validationResult );
        }
    }
}