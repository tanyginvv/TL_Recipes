using Recipes.Domain.Entities;
using Recipes.Application.Validation;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Results;
using Recipes.Application.Repositories;
using Recipes.Application.UseCases.Tags.Dtos;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Application.Paginator;

namespace Recipes.Application.UseCases.Recipes.Queries.SearchRecipe
{
    public class SearchRecipesQueryHandler(
            IRecipeRepository recipeRepository,
            IAsyncValidator<SearchRecipesQuery> validator )
        : IQueryHandler<IEnumerable<GetRecipePartDto>, SearchRecipesQuery>
    {
        public async Task<Result<IEnumerable<GetRecipePartDto>>> HandleAsync( SearchRecipesQuery query )
        {
            Result validationResult = await validator.ValidateAsync( query );
            if ( !validationResult.IsSuccess )
            {
                return Result<IEnumerable<GetRecipePartDto>>.FromError( validationResult );
            }

            PaginationFilter paginationFilter = new PaginationFilter( query.PageNumber, 4 );

            IEnumerable<Recipe> recipes;
            if ( query.SearchTerms is not null && query.SearchTerms.Any() )
            {
                recipes = await recipeRepository.GetFilteredRecipesAsync( query.SearchTerms, paginationFilter );
            }
            else
            {
                recipes = await recipeRepository.GetAllAsync( paginationFilter );
            }

            List<GetRecipePartDto> recipeDtos = recipes.Select( recipe => new GetRecipePartDto
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Description = recipe.Description,
                CookTime = recipe.CookTime,
                PortionCount = recipe.PortionCount,
                ImageUrl = recipe.ImageUrl,
                Tags = recipe.Tags.Select( t => new TagDtoUseCases
                {
                    Name = t.Name
                } ).ToList(),
            } ).ToList();

            return Result<IEnumerable<GetRecipePartDto>>.FromSuccess( recipeDtos );
        }
    }
}
