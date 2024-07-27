using Recipes.Application.Results;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Tags.Queries.GetRandomTags
{
    public class GetRandomTagsQueryValidator
        : IAsyncValidator<GetTagsForSearchQuery>
    {
        public Task<Result> ValidateAsync( GetTagsForSearchQuery query )
        {
            if ( query.Count <= 0 )
            {
                return Task.FromResult( Result.FromError( "Count must be greater than zero." ) );
            }

            return Task.FromResult( Result.Success );
        }
    }
}