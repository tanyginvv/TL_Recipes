using Recipes.Application.Results;
using Recipes.Application.Validation;

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