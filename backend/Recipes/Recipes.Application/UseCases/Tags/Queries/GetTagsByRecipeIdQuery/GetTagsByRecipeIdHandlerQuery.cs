using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Tags.Dtos;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Tags.Queries.GetTagsByRecipeIdQuery
{
    public class GetTagsByRecipeIdQueryHandler(
            ITagRepository tagRepository,
            IAsyncValidator<GetTagsByRecipeIdQuery> validator )
        : IQueryHandler<GetTagsByRecipeIdQueryDto, GetTagsByRecipeIdQuery>
    {
        public async Task<Result<GetTagsByRecipeIdQueryDto>> HandleAsync( GetTagsByRecipeIdQuery query )
        {
            Result validationResult = await validator.ValidateAsync( query );
            if ( !validationResult.IsSuccess )
            {
                return Result<GetTagsByRecipeIdQueryDto>.FromError( validationResult );
            }

            IEnumerable<Tag> tags = await tagRepository.GetByRecipeIdAsync( query.RecipeId );
            if ( tags is null )
            {
                return Result<GetTagsByRecipeIdQueryDto>.FromError( "Tags not found" );
            }

            GetTagsByRecipeIdQueryDto dto = new GetTagsByRecipeIdQueryDto
            {
                RecipeId = query.RecipeId,
                Tags = new List<Tag>( tags )
            };

            return Result<GetTagsByRecipeIdQueryDto>.FromSuccess( dto );
        }
    }
}
