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
        public async Task<Result<GetTagByNameQueryDto>> HandleAsync( GetTagByNameQuery query )
        {
            Result validationResult = await validator.ValidateAsync( query );
            if ( !validationResult.IsSuccess )
            {
                return Result<GetTagByNameQueryDto>.FromError( validationResult );
            }

            var tag = await tagRepository.GetByNameAsync( query.Name );
            if ( tag is not null )
            {
                return Result<GetTagByNameQueryDto>.FromError( "По этому названию не найдены теги" );
            }

            var dto = new GetTagByNameQueryDto
            {
                Tag = tag
            };

            return Result<GetTagByNameQueryDto>.FromSuccess( dto );
        }

    }
}
