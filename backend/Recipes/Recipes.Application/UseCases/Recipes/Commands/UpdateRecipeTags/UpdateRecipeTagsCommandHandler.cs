using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Tags.Commands.CreateTag.CreateTagCommand;
using Recipes.Application.UseCases.Tags.Dtos;
using Recipes.Application.UseCases.Tags.Queries.GetTagsByRecipeIdQuery;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Recipes.Commands.UpdateRecipeTags
{
    public class UpdateRecipeTagsCommandHandler(
            IRecipeRepository recipeRepository,
            ITagRepository tagRepository,
            IUnitOfWork unitOfWork,
            IQueryHandler<GetTagsByRecipeIdQueryDto, GetTagsByRecipeIdQuery> getTagsByRecipeIdQueryHandler,
            ICommandHandler<CreateTagCommand> createTagCommandHandler )
        : ICommandHandler<UpdateRecipeTagsCommand>
    {
        private IRecipeRepository _recipeRepository => recipeRepository;
        private ITagRepository _tagRepository => tagRepository;
        private IUnitOfWork _unitOfWork => unitOfWork;
        private IQueryHandler<GetTagsByRecipeIdQueryDto, GetTagsByRecipeIdQuery> _getTagsByRecipeIdQueryHandler => getTagsByRecipeIdQueryHandler;
        private ICommandHandler<CreateTagCommand> _createTagCommandHandler => createTagCommandHandler;

        public async Task<Result> HandleAsync( UpdateRecipeTagsCommand command )
        {
            if ( command.RecipeTags == null )
            {
                return Result.FromError( "RecipeTags cannot be null" );
            }

            var recipe = await _recipeRepository.GetByIdAsync( command.RecipeId );
            if ( recipe == null )
            {
                return Result.FromError( "Recipe not found" );
            }

            var getTagsQuery = new GetTagsByRecipeIdQuery { RecipeId = command.RecipeId };
            var queryResult = await _getTagsByRecipeIdQueryHandler.HandleAsync( getTagsQuery );

            if ( !queryResult.IsSuccess )
            {
                return Result.FromError( queryResult.Error );
            }

            var existingTags = queryResult.Value.Tags.ToList();
            var existingTagNames = existingTags.Select( t => t.Name ).ToList();

            var newTagNames = command.RecipeTags.Select( t => t.Name ).ToList();

            var tagsToRemove = existingTags.Where( t => !newTagNames.Contains( t.Name ) ).ToList();

            var tagsToAdd = new List<Tag>();
            foreach ( var name in newTagNames )
            {
                var tag = await _tagRepository.GetByNameAsync( name );
                if ( tag != null )
                {
                    if ( !existingTags.Any( t => t.Id == tag.Id ) )
                    {
                        tagsToAdd.Add( tag );
                    }
                }
                else
                {
                    var createTagCommand = new CreateTagCommand { Name = name };
                    var createResult = await _createTagCommandHandler.HandleAsync( createTagCommand );
                    if ( !createResult.IsSuccess )
                    {
                        return Result.FromError( createResult.Error );
                    }
                    tag = await _tagRepository.GetByNameAsync( name );
                    tagsToAdd.Add( tag );
                }
            }

            foreach ( var tag in tagsToRemove )
            {
                recipe.Tags.Remove( tag );
            }

            foreach ( var tag in tagsToAdd )
            {
                if ( !recipe.Tags.Contains( tag ) )
                {
                    recipe.Tags.Add( tag );
                }
            }

            await _unitOfWork.CommitAsync();

            return Result.Success;
        }
    }
}
