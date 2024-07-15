using Recipes.Application.Tags.Dtos;

namespace Recipes.Application.Recipes.Commands.UpdateRecipeTags
{
    public class UpdateRecipeTagsCommand
    {
        public int RecipeId { get; set; }
        public List<TagDto> RecipeTags { get; set; }
    }
}
