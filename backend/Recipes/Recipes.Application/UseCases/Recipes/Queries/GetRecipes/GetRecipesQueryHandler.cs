using Mapster;
using Recipes.Domain.Entities;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Results;
using Recipes.Application.Repositories;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Application.Interfaces;
using Recipes.Application.Filters;

namespace Recipes.Application.UseCases.Recipes.Queries.GetRecipes;

public class GetRecipesQueryHandler(
    IRecipeRepository recipeRepository,
    IAsyncValidator<GetRecipesQuery> validator )
    : QueryBaseHandler<IEnumerable<GetRecipePartDto>, GetRecipesQuery>( validator )
{
    protected override async Task<Result<IEnumerable<GetRecipePartDto>>> HandleAsyncImpl( GetRecipesQuery query )
    {
        List<IFilter<Recipe>> filters = new List<IFilter<Recipe>>
        {
            new SearchFilter { SearchTerms = query.SearchTerms },
            new UserFilter { UserId = query.UserId },
            new PaginationFilter { PageNumber = query.PageNumber, PageSize = 4 },
        };

        IEnumerable<Recipe> recipes = await recipeRepository.GetRecipesAsync( filters );

        List<GetRecipePartDto> recipeDtos = recipes.Adapt<List<GetRecipePartDto>>();

        return Result<IEnumerable<GetRecipePartDto>>.FromSuccess( recipeDtos );
    }
}