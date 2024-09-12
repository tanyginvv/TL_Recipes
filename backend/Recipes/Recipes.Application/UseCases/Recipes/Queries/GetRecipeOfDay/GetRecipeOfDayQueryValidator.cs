using Recipes.Application.Results;
using Recipes.Application.UseCases.Recipes.Queries.GetRecipeOfDay;
using Recipes.Application.CQRSInterfaces;

public class GetRecipeOfDayQueryValidator() : IAsyncValidator<GetRecipeOfDayQuery>
{
    public async Task<Result> ValidateAsync( GetRecipeOfDayQuery query )
    {
        return Result.Success;
    }
}