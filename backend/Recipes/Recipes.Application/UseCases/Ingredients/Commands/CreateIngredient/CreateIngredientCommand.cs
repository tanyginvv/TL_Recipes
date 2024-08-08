using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Ingredients.Commands.CreateIngredient
{
    public class CreateIngredientCommand
    {
        public required string Title { get; init; }
        public required string Description { get; init; }
        public required Recipe Recipe { get; set; }
    }
}