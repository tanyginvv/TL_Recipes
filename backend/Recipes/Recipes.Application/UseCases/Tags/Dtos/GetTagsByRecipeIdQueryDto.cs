using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Tags.Dtos
{
    public class GetTagsByRecipeIdQueryDto
    {
        public required int RecipeId { get; init; }
        public required IReadOnlyList<Tag> Tags { get; init; }
    }
}