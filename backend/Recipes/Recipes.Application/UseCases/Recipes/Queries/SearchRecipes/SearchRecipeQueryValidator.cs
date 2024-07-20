using Recipes.Application.Results;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Recipes.Queries.SearchRecipes
{
    public class SearchRecipesQueryValidator : IAsyncValidator<SearchRecipesQuery>
    {
        public async Task<Result> ValidationAsync( SearchRecipesQuery query )
        {
            return Result.Success;
        }
    }

}
