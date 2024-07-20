using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Tags.Queries.GetRandomTags
{
    public class GetRandomTagsQueryHandler( ITagRepository tagRepository,
        IAsyncValidator<GetRandomTagsQuery> validator )
        : IQueryHandler<IReadOnlyList<Tag>, GetRandomTagsQuery>
    {
        private ITagRepository _tagRepository => tagRepository;
        private IAsyncValidator<GetRandomTagsQuery> _validator => validator;

        public async Task<Result<IReadOnlyList<Tag>>> HandleAsync( GetRandomTagsQuery query )
        {
            Result validationResult = await _validator.ValidationAsync( query );

            if ( !validationResult.IsSuccess )
            {
                return Result<IReadOnlyList<Tag>>.FromError( validationResult.Error );
            }

            var tags = await _tagRepository.GetRandomTagsAsync( query.Count );

            if ( tags == null || !tags.Any() )
            {
                return Result<IReadOnlyList<Tag>>.FromError( "No tags found." );
            }

            return Result<IReadOnlyList<Tag>>.FromSuccess( tags );
        }
    }
}
