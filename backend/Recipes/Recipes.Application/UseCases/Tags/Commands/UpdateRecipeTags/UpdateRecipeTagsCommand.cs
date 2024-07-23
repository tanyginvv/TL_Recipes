using Recipes.Application.UseCases.Recipes.Dtos;

namespace Recipes.Application.UseCases.Tags.Commands.UpdateRecipeTags
{
    public class UpdateRecipeTagsCommand
    {
        public required int RecipeId { get; init; }
        public required List<TagDto> RecipeTags { get; init; }
    }
}
