using Recipes.Domain.Entities;

namespace Recipes.Application.Steps.Dtos
{
    public class GetStepsByRecipeIdQueryDto
    {
        public int RecipeId { get; init; }
        public ICollection<Step> Steps { get; set; }
    }
}