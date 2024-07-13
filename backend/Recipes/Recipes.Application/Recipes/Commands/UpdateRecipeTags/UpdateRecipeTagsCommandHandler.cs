using Application;
using Application.CQRSInterfaces;
using Application.Repositories;
using Application.Result;
using Application.Validation;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Tags.Commands.CreateTag;
using Recipes.Application.Tags.Dtos;
using Recipes.Application.Tags.Queries.GetTagsByRecipeIdQuery;
using Recipes.Domain.Entities;
using Recipes.Infrastructure.Entities.Tags;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Application.Recipes.Commands.UpdateRecipeTags
{
    public class UpdateRecipeTagsCommandHandler : ICommandHandler<UpdateRecipeTagsCommand>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQueryHandler<GetTagsByRecipeIdQueryDto, GetTagsByRecipeIdQuery> _getTagsByRecipeIdQueryHandler;
        private readonly ICommandHandler<CreateTagCommand> _createTagCommandHandler;

        public UpdateRecipeTagsCommandHandler(
            IRecipeRepository recipeRepository,
            ITagRepository tagRepository,
            IUnitOfWork unitOfWork,
            IQueryHandler<GetTagsByRecipeIdQueryDto, GetTagsByRecipeIdQuery> getTagsByRecipeIdQueryHandler,
            ICommandHandler<CreateTagCommand> createTagCommandHandler )
        {
            _recipeRepository = recipeRepository;
            _tagRepository = tagRepository;
            _unitOfWork = unitOfWork;
            _getTagsByRecipeIdQueryHandler = getTagsByRecipeIdQueryHandler;
            _createTagCommandHandler = createTagCommandHandler;
        }

        public async Task<CommandResult> HandleAsync( UpdateRecipeTagsCommand command )
        {
            if ( command.RecipeTags == null )
            {
                return new CommandResult( ValidationResult.Fail( "RecipeTags cannot be null" ) );
            }

            // Найдите рецепт по ID
            var recipe = await _recipeRepository.GetByIdAsync( command.RecipeId );
            if ( recipe == null )
            {
                return new CommandResult( ValidationResult.Fail( "Recipe not found" ) );
            }

            // Получите существующие теги рецепта через запрос
            var getTagsQuery = new GetTagsByRecipeIdQuery { RecipeId = command.RecipeId };
            var queryResult = await _getTagsByRecipeIdQueryHandler.HandleAsync( getTagsQuery );

            if ( queryResult.ValidationResult.IsFail )
            {
                return new CommandResult( queryResult.ValidationResult );
            }

            var existingTags = queryResult.ObjResult.Tags.ToList();
            var existingTagNames = existingTags.Select( t => t.Name ).ToList();

            // Преобразуйте newTagDtos в список имен тегов
            var newTagNames = command.RecipeTags.Select( t => t.Name ).ToList();

            // Найти теги, которые нужно удалить
            var tagsToRemove = existingTags.Where( t => !newTagNames.Contains( t.Name ) ).ToList();

            // Найти теги, которые нужно добавить
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
                    // Если тег не найден в базе данных, создайте новый тег
                    var createTagCommand = new CreateTagCommand { Name = name };
                    var createResult = await _createTagCommandHandler.HandleAsync( createTagCommand );
                    if ( createResult.ValidationResult.IsFail )
                    {
                        return new CommandResult( createResult.ValidationResult );
                    }
                    // Получите только что созданный тег
                    tag = await _tagRepository.GetByNameAsync( name );
                    tagsToAdd.Add( tag );
                }
            }

            // Удаление тегов
            foreach ( var tag in tagsToRemove )
            {
                recipe.Tags.Remove( tag );
            }

            // Добавление новых тегов
            foreach ( var tag in tagsToAdd )
            {
                if ( !recipe.Tags.Contains( tag ) )
                {
                    recipe.Tags.Add( tag );
                }
            }

            // Сохранение изменений
            await _unitOfWork.CommitAsync();

            return new CommandResult( ValidationResult.Ok() );
        }
    }
}
