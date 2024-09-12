using Mapster;
using Recipes.Domain.Entities;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Results;
using Recipes.Application.Repositories;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Application.Filters;

namespace Recipes.Application.UseCases.Recipes.Queries.GetRecipes;

public class GetRecipesQueryHandler(
    IRecipeRepository recipeRepository,
    IAsyncValidator<GetRecipesQuery> validator )
    : QueryBaseHandler<GetRecipesListDto, GetRecipesQuery>( validator )
{
    protected override async Task<Result<GetRecipesListDto>> HandleImplAsync( GetRecipesQuery query )
    {
        SearchFilter searchFilter = new() { SearchTerms = query.SearchTerms };
        UserFilter userFilter = new() { UserId = query.UserId, RecipeQueryType = query.RecipeQueryType };
        PaginationFilter paginationFilter = new() { PageNumber = query.PageNumber, PageSize = PaginationFilter.DefaultPageSize };

        List<IFilter<Recipe>> filter = new() { searchFilter, userFilter, paginationFilter };

        IEnumerable<Recipe> recipes = await recipeRepository.GetRecipesAsync( filter );

        paginationFilter.PageNumber += 1;
        bool nextPage = await recipeRepository.AnyAsync( filter );

        List<GetRecipePartDto> recipeDtos = recipes.Adapt<List<GetRecipePartDto>>();

        HashSet<int> likedRecipes = recipes
            .Where( r => r.Likes.Any( l => l.UserId == query.UserId ) )
            .Select( r => r.Id )
            .ToHashSet();

        HashSet<int> starredRecipes = recipes
            .Where( r => r.Favourites.Any( l => l.UserId == query.UserId ) )
            .Select( r => r.Id )
            .ToHashSet();

        foreach ( GetRecipePartDto recipeDto in recipeDtos )
        {
            recipeDto.IsLiked = likedRecipes.Contains( recipeDto.Id );
            recipeDto.IsFavourited = starredRecipes.Contains( recipeDto.Id );
        }

        GetRecipesListDto dto = new GetRecipesListDto()
        {
            GetRecipePartDtos = recipeDtos,
            IsNextPageAvailable = nextPage,
        };

        return Result<GetRecipesListDto>.FromSuccess( dto );
    }
}