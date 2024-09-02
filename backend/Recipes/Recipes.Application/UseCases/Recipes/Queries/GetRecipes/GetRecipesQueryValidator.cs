using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Results;

namespace Recipes.Application.UseCases.Recipes.Queries.GetRecipes;

public class GetRecipesQueryValidator : IAsyncValidator<GetRecipesQuery>
{
    public async Task<Result> ValidateAsync( GetRecipesQuery query )
    {
        return Result.Success;
    }
}