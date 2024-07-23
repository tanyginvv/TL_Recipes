using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Tags.Queries.GetTagsForSearch
{
    public class GetTagsForSearchQueryHandler( ITagRepository tagRepository,
        IAsyncValidator<GetTagsForSearchQuery> validator )
        : IQueryHandler<IReadOnlyList<TagDto>, GetTagsForSearchQuery>
    {
        public async Task<Result<IReadOnlyList<TagDto>>> HandleAsync( GetTagsForSearchQuery query )
        {
            Result validationResult = await validator.ValidateAsync( query );

            if ( !validationResult.IsSuccess )
            {
                return Result<IReadOnlyList<TagDto>>.FromError( validationResult.Error );
            }

            IReadOnlyList<Tag> tags = await tagRepository.GetTagsForSearchAsync( query.Count );

            if ( tags is null || !tags.Any() )
            {
                return Result<IReadOnlyList<TagDto>>.FromError( "Теги не найдены" );
            }

            List<TagDto> tagDtos = tags.Select( tag => new TagDto
            {
                Name = tag.Name
            } ).ToList();

            return Result<IReadOnlyList<TagDto>>.FromSuccess( tagDtos );
        }
    }
}