using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Tags.Commands.CreateTag;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Tags.Commands
{
    public class UpdateTagsCommandHandler(
            IRecipeRepository recipeRepository,
            ITagRepository tagRepository,
            ICommandHandlerWithResult<CreateTagCommand, Tag> createTagCommandHandler,
            IAsyncValidator<UpdateTagsCommand> validator )
        : ICommandHandler<UpdateTagsCommand>
    {
        public async Task<Result> HandleAsync( UpdateTagsCommand command )
        {
            Result validationResult = await validator.ValidateAsync( command );
            if ( !validationResult.IsSuccess )
            {
                return Result.FromError( validationResult.Error );
            }

            if ( command.RecipeTags is null || !command.RecipeTags.Any() )
            {
                return Result.FromError( "Теги рецепта не могут быть пустыми" );
            }

            Recipe recipe = await recipeRepository.GetByIdAsync( command.RecipeId );
            if ( recipe is null )
            {
                return Result.FromError( "Рецепт не найден" );
            }

            List<Tag> existingTags = recipe.Tags.ToList();
            List<string> existingTagNames = existingTags.Select( t => t.Name ).ToList();
            List<string> newTagNames = command.RecipeTags.Select( t => t.Name ).ToList();

            List<Tag> tagsToRemove = existingTags.Where( t => !newTagNames.Contains( t.Name ) ).ToList();
            List<Tag> tagsToAdd = new();

            foreach ( string name in newTagNames )
            {
                bool tagExists = await tagRepository.ContainsAsync( tag => tag.Name == name );
                if ( tagExists )
                {
                    Tag tag = await tagRepository.GetByNameAsync( name );
                    if ( !existingTags.Any( t => t.Id == tag.Id ) )
                    {
                        tagsToAdd.Add( tag );
                    }
                }
                else
                {
                    CreateTagCommand createTagCommand = new() { Name = name };
                    Result<Tag> createResult = await createTagCommandHandler.HandleAsync( createTagCommand );
                    tagsToAdd.Add( createResult.Value );
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
}
