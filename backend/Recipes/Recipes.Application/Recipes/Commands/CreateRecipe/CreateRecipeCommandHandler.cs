using Application;
using Application.Repositories;
using Application.Result;
using Application.Validation;
using Recipes.Application.CQRSInterfaces;
using Recipes.Domain.Entities;
using Recipes.Infrastructure.Entities.Steps;
using Recipes.Infrastructure.Entities.Tags;
using Recipes.Infrastructure.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Application.Recipes.Commands.CreateRecipe
{
    public class CreateRecipeCommandHandler : ICommandHandler<CreateRecipeCommand>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IStepRepository _stepRepository;
        private readonly IAsyncValidator<CreateRecipeCommand> _createRecipeCommandValidator;
        private readonly IUnitOfWork _unitOfWork;

        public CreateRecipeCommandHandler(
           IRecipeRepository recipeRepository,
           ITagRepository tagRepository,
           IIngredientRepository ingredientRepository,
           IStepRepository stepRepository,
           IAsyncValidator<CreateRecipeCommand> validator,
           IUnitOfWork unitOfWork )
        {
            _recipeRepository = recipeRepository;
            _tagRepository = tagRepository;
            _ingredientRepository = ingredientRepository;
            _stepRepository = stepRepository;
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

                foreach ( var tagName in createRecipeCommand.Tags )
                {
                    var tag = await _tagRepository.GetByNameAsync( tagName.Name );
                    if ( tag == null )
                    {
                        tag = new Tag( tagName.Name );
                        await _tagRepository.AddAsync( tag );
                    }
                    recipe.Tags.Add( tag );
                }

                await _unitOfWork.CommitAsync();

                foreach ( var ingredientDto in createRecipeCommand.Ingredients )
                {
                    Ingredient ingredient = new Ingredient(
                        ingredientDto.Title,
                        ingredientDto.Description,
                        recipe.Id
                        );

                    recipe.Ingredients.Add( ingredient );
                    await _ingredientRepository.AddIngredientAsync( ingredient );
                }

                foreach ( var stepDto in createRecipeCommand.Steps )
                {
                    Step step = new Step(
                        stepDto.StepNumber,
                        stepDto.StepDescription,
                        recipe.Id
                    );

                    recipe.Steps.Add( step );
                    await _stepRepository.AddAsync( step );
                }

                await _unitOfWork.CommitAsync();
            }

            return new CommandResult( validationResult );
        }
    }
}
