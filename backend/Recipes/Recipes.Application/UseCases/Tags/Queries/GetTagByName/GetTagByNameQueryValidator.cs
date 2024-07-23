using Recipes.Application.Results;
using Recipes.Application.UseCases.Tags.Queries.GetTagByName;
using Recipes.Application.Validation;

public class GetTagByNameQueryValidator : IAsyncValidator<GetTagByNameQuery>
{
    public async Task<Result> ValidateAsync( GetTagByNameQuery query )
    {
        if ( string.IsNullOrEmpty( query.Name ) )
        {
            return Result.FromError( "Название тега не может быть пустым" );
        }

        return Result.Success;
    }
}
