using Recipes.Domain.Entities;

namespace Recipes.Application.Tags.Dtos
{
    public class GetTagsByRecipeIdQueryDto
    {
        public int RecipeId { get; init; }
        public List<Tag> Tags { get; init; }
    }
}