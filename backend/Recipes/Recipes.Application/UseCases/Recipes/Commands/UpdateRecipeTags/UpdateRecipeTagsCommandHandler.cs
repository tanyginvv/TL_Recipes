﻿using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Tags.Commands.CreateTag.CreateTagCommand;
using Recipes.Application.UseCases.Tags.Dtos;
using Recipes.Application.UseCases.Tags.Queries.GetTagsByRecipeIdQuery;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Recipes.Commands.UpdateRecipeTags
{
    public class UpdateRecipeTagsCommandHandler(
            IRecipeRepository recipeRepository,
            ITagRepository tagRepository,
            IQueryHandler<GetTagsByRecipeIdQueryDto, GetTagsByRecipeIdQuery> getTagsByRecipeIdQueryHandler,
            ICommandHandler<CreateTagCommand> createTagCommandHandler,
            IUnitOfWork unitOfWork )
        : ICommandHandler<UpdateRecipeTagsCommand>
    {
        public async Task<Result> HandleAsync( UpdateRecipeTagsCommand command )
        {
            if ( command.RecipeTags is null )
            {
                return Result.FromError( "RecipeTags cannot be null" );
            }

            Recipe recipe = await recipeRepository.GetByIdAsync( command.RecipeId );
            if ( recipe is null )
            {
                return Result.FromError( "Recipe not found" );
            }

            List<Tag> existingTags = recipe.Tags.ToList();

            List<string> existingTagNames = existingTags.Select( t => t.Name ).ToList();

            List<string> newTagNames = command.RecipeTags.Select( t => t.Name ).ToList();

            List<Tag> tagsToRemove = existingTags.Where( t => !newTagNames.Contains( t.Name ) ).ToList();

            List<Tag> tagsToAdd = new();
            foreach ( string name in newTagNames )
            {
                Tag tag = await tagRepository.GetByNameAsync( name );
                if ( tag is not null )
                {
                    if ( !existingTags.Any( t => t.Id == tag.Id ) )
                    {
                        tagsToAdd.Add( tag );
                    }
                }
                else
                {
                    CreateTagCommand createTagCommand = new() { Name = name };
                    Result createResult = await createTagCommandHandler.HandleAsync( createTagCommand );
                    if ( !createResult.IsSuccess )
                    {
                        return Result.FromError( createResult.Error );
                    }
                    tag = await tagRepository.GetByNameAsync( name );
                    tagsToAdd.Add( tag );
                }
            }

            foreach ( Tag tag in tagsToRemove )
            {
                recipe.Tags.Remove( tag );
            }

            foreach ( Tag tag in tagsToAdd )
            {
                if ( !recipe.Tags.Contains( tag ) )
                {
                    recipe.Tags.Add( tag );
                }
            }

            await unitOfWork.CommitAsync();

            return Result.Success;
        }
    }
}
