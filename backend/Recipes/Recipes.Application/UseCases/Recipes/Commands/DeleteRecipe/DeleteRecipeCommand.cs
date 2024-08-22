namespace Recipes.Application.UseCases.Recipes.Commands.DeleteRecipe;

public class DeleteRecipeCommand
{
    public required int RecipeId { get; init; }
    public required int AuthorId { get; init; }
}