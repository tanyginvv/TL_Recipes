using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Tags.Dtos;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Tags.Queries.GetTagByName
{
    public class GetTagByNameQueryHandler(
            ITagRepository tagRepository,
            IAsyncValidator<GetTagByNameQuery> validator )
        : IQueryHandler<GetTagByNameQueryDto, GetTagByNameQuery>
    {
        private ITagRepository _tagRepository => tagRepository;
        private IAsyncValidator<GetTagByNameQuery> _tagQueryValidator => validator;

        public async Task<Result<GetTagByNameQueryDto>> HandleAsync( GetTagByNameQuery query )
        {
            Result validationResult = await _tagQueryValidator.ValidationAsync( query );
            if ( !validationResult.IsSuccess )
            {
                return Result<GetTagByNameQueryDto>.FromError( validationResult );
            }

            var tag = await _tagRepository.GetByNameAsync( query.Name );
            if ( tag != null )
            {
                return Result<GetTagByNameQueryDto>.FromError( "No tags found with the given name" );
            }

            var dto = new GetTagByNameQueryDto
            {
                Tag = tag
            };

            return Result<GetTagByNameQueryDto>.FromSuccess( dto );
        }

    }
}
