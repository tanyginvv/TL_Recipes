using Recipes.Application.Steps.Dtos;

namespace Recipes.Application.Steps.Queries
{
    public class GetStepsByRecipeIdQuery
    {
        public int RecipeId { get; init; }
        public ICollection<StepDto> Steps { get; set; }
    }
}
