using Application.CQRSInterfaces;
using Application.Result;
using Application.Validation;
using Recipes.Application.Tags.Dtos;
using Recipes.Domain.Entities;
using Recipes.Infrastructure.Entities.Tags;

namespace Recipes.Application.Tags.Queries.GetTagsByRecipeIdQuery
{
    public class GetTagsByRecipeIdQueryHandler : IQueryHandler<GetTagsByRecipeIdQueryDto, GetTagsByRecipeIdQuery>
    {
        private readonly ITagRepository _tagRepository;
        private readonly IAsyncValidator<GetTagsByRecipeIdQuery> _tagQueryValidator;

        public GetTagsByRecipeIdQueryHandler(
            ITagRepository tagRepository,
            IAsyncValidator<GetTagsByRecipeIdQuery> validator )
        {
            _tagRepository = tagRepository;
            _tagQueryValidator = validator;
        }

        public async Task<QueryResult<GetTagsByRecipeIdQueryDto>> HandleAsync( GetTagsByRecipeIdQuery query )
        {
            ValidationResult validationResult = await _tagQueryValidator.ValidationAsync( query );
            if ( validationResult.IsFail )
            {
                return new QueryResult<GetTagsByRecipeIdQueryDto>( validationResult );
            }

            IEnumerable<Tag> tags = await _tagRepository.GetByRecipeIdAsync( query.RecipeId );
            if ( tags == null )
            {
                return new QueryResult<GetTagsByRecipeIdQueryDto>( ValidationResult.Fail( "Tags not found" ) );
            }

            GetTagsByRecipeIdQueryDto dto = new GetTagsByRecipeIdQueryDto
            {
                RecipeId = query.RecipeId,
                Tags = new List<Tag>( tags )
            };

            return new QueryResult<GetTagsByRecipeIdQueryDto>( dto );
        }
    }
}
