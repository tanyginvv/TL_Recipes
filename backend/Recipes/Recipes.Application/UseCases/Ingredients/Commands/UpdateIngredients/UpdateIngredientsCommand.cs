using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Ingredients.Commands.UpdateIngredients
{
    public class UpdateIngredientsCommand
    {
        public Recipe Recipe { get; set; }
        public IEnumerable<IngredientDto> NewIngredients { get; set; }
    }
}