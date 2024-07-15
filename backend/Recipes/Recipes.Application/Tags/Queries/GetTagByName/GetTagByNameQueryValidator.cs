using Recipes.Application.Tags.Queries.GetTagByName;
using Recipes.Application.Validation;

public class GetTagByNameQueryValidator : IAsyncValidator<GetTagByNameQuery>
{
    public Task<ValidationResult> ValidationAsync( GetTagByNameQuery query )
    {
        if ( string.IsNullOrWhiteSpace( query.Name ) )
        {
            return Task.FromResult( ValidationResult.Fail( "Tag name cannot be null or empty" ) );
        }

        return Task.FromResult( ValidationResult.Ok() );
    }
}
