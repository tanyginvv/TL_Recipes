using Recipes.Domain.Entities;

namespace Recipes.Application.Recipes.Queries.GetRecipeById
{
    public class GetRecipeByIdQueryDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public int CookTime { get; init; }
        public int CountPortion { get; init; }
        public string ImageUrl { get; init; }
        public IReadOnlyList<Tag> Tags { get; init; }
        public IReadOnlyList<Step> Steps { get; init; }
        public IReadOnlyList<Ingredient> Ingredients { get; init; }

    }
}