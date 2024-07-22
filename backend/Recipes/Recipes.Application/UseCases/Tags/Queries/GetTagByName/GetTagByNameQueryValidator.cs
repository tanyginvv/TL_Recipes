using Recipes.Application.Results;
using Recipes.Application.UseCases.Tags.Queries.GetTagByName;
using Recipes.Application.Validation;

public class GetTagByNameQueryValidator : IAsyncValidator<GetTagByNameQuery>
{
    public async Task<Result> ValidateAsync( GetTagByNameQuery query )
    {
        if ( string.IsNullOrWhiteSpace( query.Name ) )
        {
            return Result.FromError( "Tag name cannot be null or empty" );
        }

        return Result.Success;
    }
}
