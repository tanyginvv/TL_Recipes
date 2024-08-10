using Mapster;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Recipes.Queries.GetRecipeById;

public class GetRecipeByIdQueryHandler(
    IRecipeRepository recipeRepository,
    IAsyncValidator<GetRecipeByIdQuery> validator )
    : IQueryHandler<GetRecipeQueryDto, GetRecipeByIdQuery>
{
    public async Task<Result<GetRecipeQueryDto>> HandleAsync( GetRecipeByIdQuery query )
    {
        Result validationResult = await validator.ValidateAsync( query );
        if ( !validationResult.IsSuccess )
        {
            return Result<GetRecipeQueryDto>.FromError( validationResult );
        }

        Recipe foundRecipe = await recipeRepository.GetByIdAsync( query.Id );

        GetRecipeQueryDto getRecipeByIdQueryDto = foundRecipe.Adapt<GetRecipeQueryDto>();

        return Result<GetRecipeQueryDto>.FromSuccess( getRecipeByIdQueryDto );
    }
}