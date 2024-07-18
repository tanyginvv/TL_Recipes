using Recipes.Application.Results;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Recipes.Queries.GetAllRecipes
{
    public class GetAllRecipesQueryValidator : IAsyncValidator<GetAllRecipesQuery>
    {
        public Task<Result> ValidationAsync( GetAllRecipesQuery query )
        {
            return Task.FromResult( Result.Success );
        }
    }
}