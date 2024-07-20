using Recipes.Application.Results;
using Recipes.Application.UseCases.Recipes.Queries.SearchRecipes;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Recipes.Queries.SearchRecipe
{
    public class SearchRecipesQueryValidator : IAsyncValidator<SearchRecipesQuery>
    {
        public async Task<Result> ValidationAsync( SearchRecipesQuery query )
        {
            return Result.Success;
        }
    }
}