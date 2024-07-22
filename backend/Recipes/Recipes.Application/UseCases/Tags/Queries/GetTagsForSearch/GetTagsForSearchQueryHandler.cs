using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Tags.Queries.GetRandomTags
{
    public class GetTagsForSearchQueryHandler( ITagRepository tagRepository,
        IAsyncValidator<GetTagsForSearchQuery> validator )
        : IQueryHandler<IReadOnlyList<Tag>, GetTagsForSearchQuery>
    {
        public async Task<Result<IReadOnlyList<Tag>>> HandleAsync( GetTagsForSearchQuery query )
        {
            Result validationResult = await validator.ValidateAsync( query );

            if ( !validationResult.IsSuccess )
            {
                return Result<IReadOnlyList<Tag>>.FromError( validationResult.Error );
            }

            IReadOnlyList<Tag> tags = await tagRepository.GetTagsForSearchAsync( query.Count );

            if ( tags is null || !tags.Any() )
            {
                return Result<IReadOnlyList<Tag>>.FromError( "No tags found." );
            }

            return Result<IReadOnlyList<Tag>>.FromSuccess( tags );
        }
    }
}