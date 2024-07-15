using Recipes.Application.Steps.Dtos;

namespace Recipes.Application.Steps.Queries.GetStepsByRecipeIdQuery
{
    public class GetStepsByRecipeIdQuery
    {
        public required int RecipeId { get; init; }
    }
}
