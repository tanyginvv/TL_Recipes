using Mapster;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Application.UseCases.Recipes.Queries.GetRecipeOfDay;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Recipes.Queries.GetRecipeById
{
    public class GetRecipeOfDayQueryHandler(
            IRecipeRepository recipeRepository,
            IAsyncValidator<GetRecipeOfDayQuery> validator )
        : IQueryHandler<GetRecipeQueryDto, GetRecipeOfDayQuery>
    {
        public async Task<Result<GetRecipeQueryDto>> HandleAsync( GetRecipeOfDayQuery query )
        {
            Result validationResult = await validator.ValidateAsync( query );
            if ( !validationResult.IsSuccess )
            {
                return Result<GetRecipeQueryDto>.FromError( validationResult );
            }

            Recipe foundRecipe = await recipeRepository.GetRecipeOfDayAsync();

            GetRecipeQueryDto getRecipeByIdQueryDto = foundRecipe.Adapt<GetRecipeQueryDto>();

            return Result<GetRecipeQueryDto>.FromSuccess( getRecipeByIdQueryDto );
        }
    }
}