using Recipes.Application.Results;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Recipes.Queries.SearchRecipe
{
    public class SearchRecipesQueryValidator : IAsyncValidator<SearchRecipesQuery>
    {
        public async Task<Result> ValidateAsync( SearchRecipesQuery query )
        {
            return Result.Success;
        }
    }
}