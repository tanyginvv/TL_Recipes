using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Ingredients.Commands.CreateIngredient;
using Recipes.Application.UseCases.Ingredients.Commands.DeleteIngredient;
using Recipes.Application.UseCases.Ingredients.Commands.UpdateIngredient;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;
using Mapster;

namespace Recipes.Application.UseCases.Ingredients.Commands.UpdateIngredients
{
    public class UpdateIngredientsCommandHandler(
        ICommandHandler<UpdateIngredientCommand> updateIngredientCommandHandler,
        ICommandHandler<DeleteIngredientCommand> deleteIngredientCommandHandler,
        ICommandHandlerWithResult<CreateIngredientCommand, Ingredient> createIngredientCommandHandler,
        IAsyncValidator<UpdateIngredientsCommand> validator )
        : ICommandHandler<UpdateIngredientsCommand>
    {
        public async Task<Result> HandleAsync( UpdateIngredientsCommand command )
        {
            Result validationResult = await validator.ValidateAsync( command );
            if ( !validationResult.IsSuccess )
            {
                return Result.FromError( validationResult.Error );
            }

            List<Ingredient> oldIngredients = command.Recipe.Ingredients.ToList();

            foreach ( IngredientDto newIngredient in command.NewIngredients )
            {
                Ingredient existingIngredient = oldIngredients.FirstOrDefault( oldIngredient => oldIngredient.Title == newIngredient.Title );
                if ( existingIngredient is null )
                {
                    CreateIngredientCommand createIngredientCommand = newIngredient.Adapt<CreateIngredientCommand>();
                    createIngredientCommand.Recipe = command.Recipe;

                    await createIngredientCommandHandler.HandleAsync( createIngredientCommand );
                }
                else if ( existingIngredient.Description != newIngredient.Description )
                {
                    UpdateIngredientCommand updateIngredientCommand = newIngredient.Adapt<UpdateIngredientCommand>();
                    updateIngredientCommand.Id = existingIngredient.Id;

                    await updateIngredientCommandHandler.HandleAsync( updateIngredientCommand );
                }
            }

            List<Ingredient> ingredientsToDelete = oldIngredients.Where( oldIngredient => !command.NewIngredients.Any( newIngredient => newIngredient.Title == oldIngredient.Title ) ).ToList();
            foreach ( Ingredient ingredientToDelete in ingredientsToDelete )
            {
                DeleteIngredientCommand deleteIngredientCommand = new DeleteIngredientCommand { Id = ingredientToDelete.Id };
                await deleteIngredientCommandHandler.HandleAsync( deleteIngredientCommand );
            }

            return Result.Success;
        }
    }
}
