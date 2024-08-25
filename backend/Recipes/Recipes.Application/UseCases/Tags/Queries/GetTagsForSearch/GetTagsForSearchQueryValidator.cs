using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Results;

namespace Recipes.Application.UseCases.Tags.Queries.GetTagsForSearch;

public class GetRandomTagsQueryValidator
    : IAsyncValidator<GetTagsForSearchQuery>
{
    public Task<Result> ValidateAsync( GetTagsForSearchQuery query )
    {
        if ( query.Count <= 0 )
        {
            return Task.FromResult( Result.FromError( "Тегов должно быть больше 0" ) );
        }

        return Task.FromResult( Result.Success );
    }
}