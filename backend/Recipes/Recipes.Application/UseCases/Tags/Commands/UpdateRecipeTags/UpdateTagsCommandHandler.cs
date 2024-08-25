using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Tags.Commands.GetOrCreateTag;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Tags.Commands.UpdateRecipeTags;

public class UpdateTagsCommandHandler(
    IRecipeRepository recipeRepository,
    ITagRepository tagRepository,
    ICommandHandlerWithResult<GetOrCreateTagCommand, Tag> createTagCommandHandler,
    IAsyncValidator<UpdateTagsCommand> validator )
    : CommandBaseHandler<UpdateTagsCommand>(validator)
{
    protected override async Task<Result> HandleAsyncImpl( UpdateTagsCommand command )
    {
        Recipe recipe = await recipeRepository.GetByIdAsync( command.RecipeId );
        if ( recipe is null )
        {
            return Result.FromError( "Рецепт не найден" );
        }

        List<Tag> existingTags = recipe.Tags.ToList();
        List<string> newTagNames = command.RecipeTags.Select( t => t.Name ).ToList();

        List<Tag> tagsToRemove = existingTags.Where( t => !newTagNames.Contains( t.Name ) ).ToList();
        List<Tag> tagsToAdd = new();

        foreach ( string name in newTagNames )
        {
            Tag tag = await tagRepository.GetByNameAsync( name );
            if ( tag is null )
            {
                GetOrCreateTagCommand createTagCommand = new() { Name = name };
                Result<Tag> createResult = await createTagCommandHandler.HandleAsync( createTagCommand );
                tagsToAdd.Add( createResult.Value );
            }
            else
            {
                if ( !existingTags.Any( t => t.Id == tag.Id ) )
                {
                    tagsToAdd.Add( tag );
                }
            }
        }

        foreach ( Tag tag in tagsToRemove )
        {
            recipe.Tags.Remove( tag );
        }

        foreach ( Tag tag in tagsToAdd )
        {
            if ( !recipe.Tags.Any( t => t.Id == tag.Id ) )
            {
                recipe.Tags.Add( tag );
            }
        }

        return Result.Success;
    }
}