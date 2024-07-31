using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Ingredients.Commands
{
    public class UpdateIngredientsCommand
    {
        public Recipe Recipe { get; set; }
        public IEnumerable<IngredientDto> NewIngredients { get; set; }
    }
}
