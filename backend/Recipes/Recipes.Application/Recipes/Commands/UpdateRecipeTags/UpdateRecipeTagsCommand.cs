using Recipes.Application.Tags.Dtos;

namespace Recipes.Application.Recipes.Commands.UpdateRecipeTags
{
    public class UpdateRecipeTagsCommand
    {
        public required int RecipeId { get; set; }
        public required List<TagDto> RecipeTags { get; set; }
    }
}
