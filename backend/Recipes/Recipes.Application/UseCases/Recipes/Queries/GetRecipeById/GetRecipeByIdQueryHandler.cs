using Mapster;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Ingredients.Dtos;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Recipes.Queries.GetRecipeById
{
    public class GetRecipeByIdQueryHandler(
            IRecipeRepository recipeRepository,
            IAsyncValidator<GetRecipeByIdQuery> validator )
        : IQueryHandler<GetRecipeByIdQueryDto, GetRecipeByIdQuery>
    {
        public async Task<Result<GetRecipeByIdQueryDto>> HandleAsync( GetRecipeByIdQuery query )
        {
            Result validationResult = await validator.ValidateAsync( query );
            if ( !validationResult.IsSuccess )
            {
                return Result<GetRecipeByIdQueryDto>.FromError( validationResult );
            }

            Recipe foundRecipe = await recipeRepository.GetByIdAsync( query.Id );

            GetRecipeByIdQueryDto getRecipeByIdQueryDto = foundRecipe.Adapt<GetRecipeByIdQueryDto>();

            return Result<GetRecipeByIdQueryDto>.FromSuccess( getRecipeByIdQueryDto );
        }
    }
}
