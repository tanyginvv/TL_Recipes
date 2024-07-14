using Application.CQRSInterfaces;
using Application.Result;
using Application.Validation;
using Recipes.Infrastructure.Repositories;

namespace Recipes.Application.Tags.Queries.GetTagsByName
{
    public class GetTagByNameQueryHandler : IQueryHandler<GetTagByNameQueryDto, GetTagByNameQuery>
    {
        private readonly ITagRepository _tagRepository;
        private readonly IAsyncValidator<GetTagByNameQuery> _tagQueryValidator;

        public GetTagByNameQueryHandler(
            ITagRepository tagRepository,
            IAsyncValidator<GetTagByNameQuery> validator )
        {
            _tagRepository = tagRepository;
            _tagQueryValidator = validator;
        }

        public async Task<QueryResult<GetTagByNameQueryDto>> HandleAsync( GetTagByNameQuery query )
        {
            ValidationResult validationResult = await _tagQueryValidator.ValidationAsync( query );
            if ( validationResult.IsFail )
            {
                return new QueryResult<GetTagByNameQueryDto>( validationResult );
            }

            var tag = await _tagRepository.GetByNameAsync( query.Name );
            if ( tag != null )
            {
                return new QueryResult<GetTagByNameQueryDto>( ValidationResult.Fail( "No tags found with the given name" ) );
            }

            var dto = new GetTagByNameQueryDto
            {
                Tag = tag
            };

            return new QueryResult<GetTagByNameQueryDto>( dto );
        }

    }
}
