using Recipes.Domain.Entities;

namespace Recipes.Application.Tags.Dtos
{
    public class GetTagsByRecipeIdQueryDto
    {
        public int RecipeId { get; init; }
        public IReadOnlyList<Tag> Tags { get; init; }
    }
}